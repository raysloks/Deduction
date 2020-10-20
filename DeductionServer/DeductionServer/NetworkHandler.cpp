#include "NetworkHandler.h"

#include <iostream>

NetworkHandler::NetworkHandler() : game(*this)
{
	link.handler = this;
	link.Open(asio::ip::udp::endpoint(asio::ip::udp::v4(), 0));
	link.Receive();

	createMob();
}

void NetworkHandler::tick(const std::chrono::steady_clock::time_point & now)
{
	time = now.time_since_epoch().count();

	game.tick(time);

	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = mobs[i];
		if (mob.enabled)
		{
			MobUpdate message;
			message.id = i;
			message.time = time;
			if (mob.time)
				message.time = mob.time;
			message.position = mob.position;
			for (auto player : players)
			{
				if (mob.visibleTo(mobs[player.second.mob]))
				{
					link.Send(player.first, message);
				}
			}
		}
	}

	auto players_copy = players;
	for (auto player : players_copy)
	{
		if (time > player.second.timeout)
		{
			players.erase(player.first);
			removeMob(player.second.mob);

			std::cout << player.second.name << " at " << player.first << " timed out" << std::endl;
		}
	}
}

uint64_t NetworkHandler::createMob(const Mob& mob)
{
	size_t i = 0;
	for (; i < mobs.size(); ++i)
		if (!mobs[i].enabled)
			break;
	if (i == mobs.size())
		mobs.resize(i + 1);
	mobs[i] = mob;
	mobs[i].enabled = true;
	updateMobState(i);
	return i;
}

void NetworkHandler::createPlayer(const asio::ip::udp::endpoint & endpoint, const std::string & name)
{
	Player player;
	player.mob = createMob();
	player.name = name;
	player.timeout = time + 10'000'000'000;
	players.emplace(endpoint, player);

	mobs[player.mob].color = Vec3(
		rng.next_float() * 0.6f + 0.2f,
		rng.next_float() * 0.6f + 0.2f,
		rng.next_float() * 0.6f + 0.2f
	);

	{
		PlayerUpdate message;
		message.id = 0;
		message.mob = player.mob;
		message.name = player.name;
		Broadcast(message);
	}

	for (auto player : players)
	{
		PlayerUpdate message;
		message.id = 0;
		message.mob = player.second.mob;
		message.name = player.second.name;
		link.Send(endpoint, message);
	}

	{
		GamePhaseUpdate message;
		message.phase = (uint64_t)game.phase;
		message.timer = game.timer;
		link.Send(endpoint, message);
	}

	{
		PlayerUpdate message;
		message.id = -1;
		message.mob = players[endpoint].mob;
		link.Send(endpoint, message);
	}

	{
		GameSettingsUpdate message;
		message.values = std::vector<int64_t>(std::begin(game.settings.settings), std::end(game.settings.settings));
		link.Send(endpoint, message);
	}

	updateMobStatesForPlayer(endpoint);

	std::cout << name << " connected from " << endpoint << std::endl;
}

void NetworkHandler::updateMobState(uint64_t id)
{
	auto&& mob = mobs[id];

	MobStateUpdate message;
	message.update.id = id;
	message.update.time = time;
	message.update.position = mob.position;
	message.color = mob.color;
	message.type = (uint64_t)mob.type;

	for (auto player : players)
	{
		if (mob.visibleTo(mobs[player.second.mob]))
		{
			link.Send(player.first, message);
		}
	}
}

void NetworkHandler::updateMobStatesForPlayer(const asio::ip::udp::endpoint & endpoint)
{
	auto&& player = players[endpoint];

	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = mobs[i];
		if (mob.enabled)
		{
			if (mob.visibleTo(mobs[player.mob]))
			{
				MobStateUpdate message;
				message.update.id = i;
				message.update.time = time;
				message.update.position = mob.position;
				message.color = mob.color;
				message.type = (uint64_t)mob.type;
				link.Send(endpoint, message);
			}
		}
	}
}

void NetworkHandler::updateMobStates()
{
	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = mobs[i];
		if (mob.enabled)
		{
			updateMobState(i);
		}
	}
}

void NetworkHandler::updateMobRoles()
{
	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = mobs[i];
		if (mob.enabled)
		{
			for (auto player : players)
			{
				MobRoleUpdate message;
				message.id = i;
				switch (mobs[player.second.mob].role)
				{
				case Role::Crewmate:
					message.role = Role::Crewmate;
					break;
				case Role::Impostor:
					message.role = mob.role;
					break;
				default:
					break;
				}
				link.Send(player.first, message);
			}
		}
	}
}

void NetworkHandler::removeMob(uint64_t id)
{
	auto&& mob = mobs[id];
	if (mob.enabled)
	{
		mob.enabled = false;

		MobRemoved message;
		message.id = id;
		message.time = time;
		Broadcast(message);
	}
}

void NetworkHandler::killMob(uint64_t id)
{
	auto&& mob = mobs[id];
	mob.type = MobType::Ghost;

	updateMobState(id);

	Mob corpse;
	corpse.position = mob.position;
	corpse.color = mob.color;
	corpse.type = MobType::Corpse;
	corpse.role = mob.role;
	createMob(corpse);

	MobRemoved message;
	message.id = id;
	message.time = time;

	for (auto player : players)
	{
		if (player.second.mob == id)
			updateMobStatesForPlayer(player.first);
		if (!mob.visibleTo(mobs[player.second.mob]))
		{
			link.Send(player.first, message);
		}
	}
}

void NetworkHandler::ConnectionHandler(const asio::ip::udp::endpoint & endpoint)
{
}

void NetworkHandler::AbilityUsedHandler(const asio::ip::udp::endpoint & endpoint, const AbilityUsed & message)
{
}

void NetworkHandler::GamePhaseUpdateHandler(const asio::ip::udp::endpoint & endpoint, const GamePhaseUpdate & message)
{
	if (message.phase == 1 && game.phase != GamePhase::Main)
	{
		game.removeCorpses();
		game.setPhase(GamePhase::Main, message.timer);
	}
	else if (message.phase == 2 && game.phase != GamePhase::Meeting)
	{
		game.setPhase(GamePhase::Meeting, message.timer);
	}
}

void NetworkHandler::GameSettingSetHandler(const asio::ip::udp::endpoint & endpoint, const GameSettingSet & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		game.settings.settings[message.setting] = message.value;
		Broadcast(message);
	}
}

void NetworkHandler::GameSettingsUpdateHandler(const asio::ip::udp::endpoint & endpoint, const GameSettingsUpdate & message)
{
	if (game.phase == GamePhase::Setup)
	{
		game.settings = GameSettings();

		GameSettingsUpdate message;
		message.values = std::vector<int64_t>(std::begin(game.settings.settings), std::end(game.settings.settings));
		Broadcast(message);
	}
}

void NetworkHandler::GameStartRequestedHandler(const asio::ip::udp::endpoint & endpoint, const GameStartRequested & message)
{
	game.startGameCountdown();
	Broadcast(message);
}

void NetworkHandler::HeartbeatHandler(const asio::ip::udp::endpoint & endpoint, const Heartbeat & message)
{
	Heartbeat reply;
	//reply.time = time;
	reply.time = std::chrono::steady_clock::now().time_since_epoch().count();
	link.Send(endpoint, reply);

	auto it = players.find(endpoint);
	if (it != players.end())
	{
		it->second.timeout = time + 5'000'000'000;
	}
}

void NetworkHandler::KillAttemptedHandler(const asio::ip::udp::endpoint & endpoint, const KillAttempted & message)
{
	if (game.phase != GamePhase::Main)
		return;

	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];
		if (mob.type == MobType::Player && mob.role == Role::Impostor && mob.killCooldown < time)
		{
			if (message.target < mobs.size())
			{
				auto&& target = mobs[message.target];
				if (target.enabled && target.type == MobType::Player && target.role == Role::Crewmate)
				{
					if ((target.position - mob.position).Len() < 2.0f)
					{
						killMob(message.target);

						MobTeleport message;
						message.from = mob.position;
						message.to = target.position;
						message.time = time;
						message.id = player.mob;
						Broadcast(message);

						mob.position = message.to;
						mob.killCooldown = time + game.settings.killCooldown;

						KillAttempted reply;
						reply.time = mob.killCooldown;
						link.Send(endpoint, reply);

						game.checkForGameOver();
					}
				}
			}
		}
	}
}

void NetworkHandler::MeetingRequestedHandler(const asio::ip::udp::endpoint & endpoint, const MeetingRequested & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (message.EmergencyMeetings > mob.meetingsCalled)
		{
			for (auto mob : mobs)
			{
				mob.timesVoted = 0;
			}
			game.startMeeting();

			mob.meetingsCalled++;
			if (message.EmergencyMeetings == mob.meetingsCalled)
			{
				MeetingRequested message;
				message.idOfInitiator = player.mob;
				message.EmergencyMeetings = mob.meetingsCalled;

			    Broadcast(message);
			}
			else
			{
				MeetingRequested message;
				message.idOfInitiator = player.mob;

				Broadcast(message);
			}
		}
	}
}

void NetworkHandler::MobRemovedHandler(const asio::ip::udp::endpoint & endpoint, const MobRemoved & message)
{
	killMob(message.id);
}

void NetworkHandler::MobRoleUpdateHandler(const asio::ip::udp::endpoint & endpoint, const MobRoleUpdate & message)
{
}

void NetworkHandler::MobStateUpdateHandler(const asio::ip::udp::endpoint & endpoint, const MobStateUpdate & message)
{
}

void NetworkHandler::MobTeleportHandler(const asio::ip::udp::endpoint & endpoint, const MobTeleport & message)
{
}

void NetworkHandler::MobUpdateHandler(const asio::ip::udp::endpoint & endpoint, const MobUpdate & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];
		mob.position = message.position;
mob.time = message.time;
	}
}

void NetworkHandler::PlayerUpdateHandler(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message)
{
	auto it = players.find(endpoint);
	if (it == players.end())
	{
		createPlayer(endpoint, message.name);
	}
}

void NetworkHandler::PlayerVotedHandler(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		int totalvotes = (int)message.totalVotes;

		if (totalvotes > mob.timesVoted)
		{
			mob.timesVoted++;
			int phase2 = 2;
			int alive = 0;
			int total = 0;

			for (auto mob : mobs)
			{
				if (mob.type == MobType::Player && mob.enabled == true && (mob.role == Role::Crewmate || mob.role == Role::Impostor))
				{
					alive++;
					total += mob.timesVoted;
				}

			}
			if (alive * totalvotes <= total)
			{
				//game.setPhase(GamePhase::Main, message.timer);
				phase2 = 1;
			}
			std::string name = message.buttonName;
			PlayerVoted message;
			message.phase = phase2;
			message.timer = time;
			message.id = player.mob;
			message.totalVotes = totalvotes;
			message.buttonName = name;
			message.votesLeft = mob.timesVoted - totalvotes;
			Broadcast(message);
		}
	}
}

void NetworkHandler::ReportAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		game.startMeeting();

		ReportAttempted message;
		message.idOfInitiator = player.mob;
		message.target = message.target;

		Broadcast(message);
	}
}

void NetworkHandler::RestartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message)
{
	game.restartSetup();
}

void NetworkHandler::TaskListUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message)
{
}

void NetworkHandler::TaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message)
{
}

void NetworkHandler::VoiceFrameHandler(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		VoiceFrame frame = message;
		frame.id = player.mob;
		Broadcast(frame);
	}
}
void NetworkHandler::GivenTasksHandler(const asio::ip::udp::endpoint& endpoint, const GivenTasks& message) {

}
void NetworkHandler::GameOverHandler(const asio::ip::udp::endpoint& endpoint, const GameOver& message){

}

//void NetworkHandler::GivenTasksHandler(const asio::ip::udp::endpoint & endpoint, const GivenTasks & message) 
//{
//	auto it = players.find(endpoint);
//	if (it != players.end())
//	{
//		auto&& player = it->second;
//		GivenTasks message;
//		message.taskId = new int[5];
//		for (size_t i = 0; i < mobs.size(); ++i)
//		{
//			for (int i = 0; i < 5; ++i) 
//			{
//				message.taskId[i] = 0;
//			}
//			for (int i = 0; i < 5; ++i)
//			{
//				
//			}
//		}
//	}
//}
