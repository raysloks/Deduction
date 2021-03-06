#include "NetworkHandler.h"

#include <iostream>

#include "MatchmakerHandler.h"

NetworkHandler::NetworkHandler() : game(*this)
{
	link.handler = this;
	link.Open(asio::ip::udp::endpoint(asio::ip::udp::v4(), 0));
	link.Receive();

	mobs.reserve(1024); // lul

	leader = 0;
	lifetimePlayerCount = 0;

	vacant = false; // immediately acquire new lobby

	//createMob();
}

void NetworkHandler::tick(const std::chrono::steady_clock::time_point & now)
{
	std::lock_guard lock(link.mutex);

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

			PlayerUpdate message;
			message.id = player.second.index;
			message.mob = player.second.mob;
			message.name = "";
			Broadcast(message);

			std::cout << player.second.name << " at " << player.first << " timed out" << std::endl;

			if (player.second.index == leader)
			{
				leader = lifetimePlayerCount;
				for (auto player : players)
				{
					if (player.second.index < leader)
						leader = player.second.index;
				}

				for (auto player : players)
				{
					if (player.second.index == leader)
					{
						PlayerUpdate message;
						message.id = -1ul;
						message.mob = player.second.mob;
						link.Send(player.first, message);
					}
				}
			}
		}
	}

	if (players.empty() && !vacant)
	{
		matchmaker->UpdateLobby();
		vacant = true;
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
	vacant = false;

	Mob mob;
	if (game.phase != GamePhase::Setup)
		mob.type = MobType::Ghost;
	mob.sprite = getUnusedSprite();

	Player player;
	player.index = lifetimePlayerCount;
	player.mob = createMob(mob);
	player.name = name;
	player.timeout = time + 10'000'000'000;
	players.emplace(endpoint, player);

	++lifetimePlayerCount;

	/*mobs[player.mob].color = generateColor();*/

	{
		PlayerUpdate message;
		message.id = player.index;
		message.mob = player.mob;
		message.name = player.name;
		Broadcast(message);
	}

	for (auto player : players)
	{
		PlayerUpdate message;
		message.id = player.second.index;
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
		message.id = player.index == leader ? -1ul : -2ul;
		message.mob = players[endpoint].mob;
		link.Send(endpoint, message);
	}

	link.Send(endpoint, game.settings);

	updateMobState(player.mob);
	updateMobStatesForPlayer(endpoint);

	std::cout << name << " connected from " << endpoint << std::endl;
}

//Vec3 NetworkHandler::generateColor()
//{
//	std::vector<Vec3> colors(10);
//	for (size_t i = 0; i < colors.size(); ++i)
//	{
//		colors[i] = Vec3(
//			rng.next_float() * 0.8f + 0.1f,
//			rng.next_float() * 0.8f + 0.1f,
//			rng.next_float() * 0.8f + 0.1f
//		);
//	}
//
//	std::vector<float> distances(colors.size(), std::numeric_limits<float>::infinity());
//	for (size_t i = 0; i < colors.size(); ++i)
//	{
//		for (auto player : players)
//		{
//			float distance = (colors[i] - mobs[player.second.mob].color).Len();
//			if (distance < distances[i])
//				distances[i] = distance;
//		}
//	}
//
//	size_t most_distant_index = 0;
//	for (size_t i = 1; i < distances.size(); ++i)
//	{
//		if (distances[i] > distances[most_distant_index])
//			most_distant_index = i;
//	}
//
//	return colors[most_distant_index];
//}

void NetworkHandler::updateMobState(uint64_t id)
{
	auto&& mob = mobs[id];

	MobStateUpdate message;
	message.update.id = id;
	message.update.time = time;
	message.update.position = mob.position;
	message.color = mob.color;
	message.type = (uint64_t)mob.type;
	message.sprite = mob.sprite;

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
				message.sprite = mob.sprite;
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

void NetworkHandler::killMob(uint64_t id, uint64_t killer, bool knife)
{
	auto&& mob = mobs[id];
	mob.type = MobType::Ghost;

	updateMobState(id);

	if (killer == -1ul)
	{
		MobEjected message;
		message.id = id;
		Broadcast(message);
	}
	else
	{
		Mob corpse;
		corpse.position = mob.position;
		corpse.color = mob.color;
		corpse.type = MobType::Corpse;
		corpse.role = mob.role;
		corpse.sprite = mob.sprite;
		createMob(corpse);

		KillAttempted message;
		message.time = time + game.settings.killCooldown;
		message.target = id;
		message.killer = killer;
		message.knife = knife;

		for (auto player : players)
		{
			if (player.second.mob == id || player.second.mob == killer)
				link.Send(player.first, message);
		}
	}

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

uint64_t NetworkHandler::getUnusedSprite() const
{
	for (size_t i = 0; i < players.size(); ++i)
	{
		auto it = std::find_if(players.begin(), players.end(), [this, i](auto player)
			{
				return mobs[player.second.mob].sprite == i;
			}
		);
		if (it == players.end())
			return i;
	}
	return players.size();
}

void NetworkHandler::AcceptHandler(const asio::ip::udp::endpoint & endpoint)
{
}

void NetworkHandler::ConnectHandler(const asio::ip::udp::endpoint & endpoint)
{
}

void NetworkHandler::AbilityUsedHandler(const asio::ip::udp::endpoint & endpoint, const AbilityUsed & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];
		if (game.phase == GamePhase::Main && mob.role == Role::Impostor && mob.sabotageCooldown < time)
		{
			if (game.callSabotage(message.ability))
			{
				mob.sabotageCooldown = time + game.settings.sabotageCooldown;
				AbilityUsed reply;
				reply.time = mob.sabotageCooldown;
				Send(endpoint, reply);
			}
		}
	}
}

void NetworkHandler::GameSettingsHandler(const asio::ip::udp::endpoint & endpoint, const GameSettings & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		if (game.phase == GamePhase::Setup && player.index == leader)
		{
			game.settings = message;
			game.checkForMapChange();
			Broadcast(game.settings);
		}
	}
}

void NetworkHandler::GameStartRequestedHandler(const asio::ip::udp::endpoint & endpoint, const GameStartRequested & message)
{
	auto it = players.find(endpoint);	
	if (it != players.end())
	{
		auto&& player = it->second;
		if (player.index == leader)
		{
			game.startGameCountdown();
		}
	}
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
		if (mob.type == MobType::Player)
		{
			if (message.target < mobs.size())
			{
				auto&& target = mobs[message.target];
				if (target.enabled && target.type == MobType::Player)
				{
					if ((target.position - mob.position).Len() < game.settings.killRange + game.settings.playerSpeed * 0.1f)
					{
						killMob(message.target, player.mob, message.knife);

						MobTeleport reply;
						reply.from = mob.position;
						reply.to = target.position;
						reply.time = time;
						reply.id = player.mob;
						Broadcast(reply);

						mob.position = reply.to;
						mob.killCooldown = time + game.settings.killCooldown;

						game.checkForGameOver(time);
					}
				}
			}
		}
	}
}

void NetworkHandler::MeetingRequestedHandler(const asio::ip::udp::endpoint & endpoint, const MeetingRequested & message)
{
	std::cout << "Meeting Started" << std::endl;;

	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.type == MobType::Player && mob.meetingsCalled < game.settings.emergencyMeetingsPerPlayer)
		{
			++mob.meetingsCalled;
			game.startMeeting(player.mob);
		}
	}
}

void NetworkHandler::MobUpdateHandler(const asio::ip::udp::endpoint & endpoint, const MobUpdate & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];
		mob.flipped = message.flipped;
		mob.position = message.position;
		mob.time = message.time;
	}
}

void NetworkHandler::PhotoTakenHandler(const asio::ip::udp::endpoint & endpoint, const PhotoTaken & message)
{
}

void NetworkHandler::PlayerUpdateHandler(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		if (game.phase == GamePhase::Setup)
		{
			auto&& player = it->second;

			player.name = message.name;

			PlayerUpdate message;
			message.id = 0;
			message.mob = player.mob;
			message.name = player.name;
			Broadcast(message);
		}
	}
	else
	{
		createPlayer(endpoint, message.name);
	}
}

void NetworkHandler::PlayerVotedHandler(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message)
{
	if (game.phase != GamePhase::Voting)
		return;

	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.votesCast < game.settings.votesPerPlayer && mob.type == MobType::Player)
		{
			mob.votesCast++;
			game.votes.push_back(std::make_pair(player.mob, message.target));

			int alive = 0;
			int total = 0;
			for (auto mob : mobs)
			{
				if (mob.type == MobType::Player && mob.enabled == true)
				{
					alive++;
					total += mob.votesCast;
				}
			}

			if (total == alive * game.settings.votesPerPlayer)
			{
				game.endMeeting(time);
			}

			PlayerVoted reply;
			if (game.settings.anonymousVotes)
				reply.voter = -1;
			else
				reply.voter = player.mob;
			if (game.settings.showVotesWhenEveryoneHasVoted)
				reply.target = -2;
			else
				reply.target = message.target;
			Broadcast(reply);
		}
	}
}

void NetworkHandler::PresentEvidenceHandler(const asio::ip::udp::endpoint & endpoint, const PresentEvidence & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.type == MobType::Player && game.phase == GamePhase::Discussion || game.phase == GamePhase::Voting)
		{
			if (message.index < game.photos.size())
			{
				auto&& photo = game.photos[message.index];
				if (photo.poses[photo.photographer].index == player.mob)
				{
					PresentEvidence reply;
					reply.index = message.index;
					reply.presenter = player.mob;
					Broadcast(reply);
				}
			}
		}
	}
}

void NetworkHandler::ReportAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.type == MobType::Player)
		{
			if (message.target < mobs.size())
			{
				auto&& target = mobs[message.target];
				if (target.type == MobType::Corpse)
					game.startMeeting(player.mob, message.target);
			}
		}
	}
}

void NetworkHandler::ResetGameSettingsHandler(const asio::ip::udp::endpoint & endpoint, const ResetGameSettings & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		if (player.index == leader)
		{
			game.resetSettings();
		}
	}
}

void NetworkHandler::RestartRequestedHandler(const asio::ip::udp::endpoint & endpoint, const RestartRequested & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		if (player.index == leader)
		{
			game.restartSetup();
		}
	}
}

void NetworkHandler::SabotageTaskUpdateHandler(const asio::ip::udp::endpoint & endpoint, const SabotageTaskUpdate & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.type == MobType::Player)
			game.fixSabotage(message.sabotage);
	}
}

void NetworkHandler::TaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (message.task < mob.tasks.size())
		{
			mob.tasks[message.task].completed = true;
			if (game.settings.taskbarUpdateStyle == 0)
				game.updateTaskbar();
			game.checkForGameOver(time);
		}
	}
}

void NetworkHandler::VoiceFrameHandler(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];
		VoiceFrame frame = message;
		frame.id = player.mob;
		if (mob.type == MobType::Ghost)
		{
			for (auto [endpoint, player] : players)
			{
				auto&& mob = mobs[player.mob];
				if (mob.type == MobType::Ghost)
					Send(endpoint, frame);
			}
		}
		else
		{
			if (game.voiceEnabled)
			{
				Broadcast(frame);
			}
			else
			{
				if (mob.role == Role::Impostor)
				{
					for (auto [endpoint, player] : players)
					{
						auto&& mob = mobs[player.mob];
						if (mob.role == Role::Impostor ||
							mob.type == MobType::Ghost ||
							player.mob == frame.id)
							Send(endpoint, frame);
					}
				}
			}
		}
	}
}

void NetworkHandler::SendEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const SendEvidence& message)
{
}

void NetworkHandler::SendSensorListHandler(const asio::ip::udp::endpoint& endpoint, const SendSensorList& message)
{
	Broadcast(message);
}

void NetworkHandler::SetMobColorHandler(const asio::ip::udp::endpoint& endpoint, const SetMobColor& message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		mob.color = message.color;
		updateMobState(player.mob);
		
		changeColor(message.id, message.former);
	}
}

void NetworkHandler::changeColor(uint32_t id, uint32_t former){
	ColorTaken message;
	message.former = former;
	message.id = id;
	Broadcast(message);
}

void NetworkHandler::TakePhotoHandler(const asio::ip::udp::endpoint & endpoint, const TakePhoto & message)
{
	auto it = players.find(endpoint);
	if (it != players.end())
	{
		auto&& player = it->second;
		auto&& mob = mobs[player.mob];

		if (mob.type == MobType::Player)
		{
			game.takePhoto(player.mob);
		}
	}
}

void NetworkHandler::PickupCooldownHandler(const asio::ip::udp::endpoint& endpoint, const PickupCooldown& message)
{
	Broadcast(message);
}

void NetworkHandler::TeleportToMeetingHandler(const asio::ip::udp::endpoint& endpoint, const TeleportToMeeting& message)
{
	game.teleportToMeeting();
}

void NetworkHandler::HideAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const HideAttempted& message)
{
	HideAttempted hide = message;
	Broadcast(hide);
}

void NetworkHandler::GetAllPlayerPositionsHandler(const asio::ip::udp::endpoint& endpoint, const GetAllPlayerPositions& message)
{	
	auto it = players.find(endpoint);

	if (it != players.end())
	{
		auto&& player = it->second;
		std::vector<Vec3> playerPos = game.GetPlayersPos();
		GetAllPlayerPositions Pp;
		Pp.playerPos = playerPos;
		Pp.id = player.mob;
		Pp.player = mobs[player.mob].position;
		Send(endpoint, Pp);
	}
}

void NetworkHandler::SmokeGrenadeActivateHandler(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeActivate& message)
{
	Broadcast(message);
}

void NetworkHandler::PulseEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const PulseEvidence& message)
{
	Broadcast(message);
}

void NetworkHandler::SmokeGrenadeEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeEvidence& message)
{
	Broadcast(message);
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
