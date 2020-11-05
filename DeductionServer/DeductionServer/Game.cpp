#include "Game.h"

#include "NetworkHandler.h"

#include "SabotageTask.h"
#include "LightSabotage.h"
#include "DoorSabotage.h"
#include "VoiceSabotage.h"

#include "Coal.h"

Game::Game(NetworkHandler& handler) : handler(handler)
{
	phase = GamePhase::Setup;
	timer = 0;

	voiceEnabled = true;

	std::ifstream f("../../../maps.txt");

	auto coal = Coal::parse(f);
	coal->print(std::cout);
	for (auto element : coal->elements)
		maps.push_back(std::make_shared<Map>(element));

	if (maps.empty())
	{
		maps.push_back(std::make_shared<Map>());
		std::cout << "unable to load map data." << std::endl;
	}

	map = maps[0];
	settings.map = 0;
	resetSettings();
}

void Game::tick(int64_t now)
{
	if (now > timer && timer != 0)
	{
		switch (phase)
		{
		case GamePhase::Setup:
			startGame();
			break;
		case GamePhase::Intro:
			resetCooldowns();
			setPhase(GamePhase::Main, 0);
			break;
		case GamePhase::Main:
			break;
		case GamePhase::Discussion:
			setPhase(GamePhase::Voting, timer + settings.voteTime);
			break;
		case GamePhase::Voting:
			endMeeting(timer);
			break;
		case GamePhase::EndOfMeeting:
			setPhase(GamePhase::Ejection, timer + 3'000'000'000);
			for (auto i : toBeEjected)
				handler.killMob(i, -1ul);
			toBeEjected.clear();
			break;
		case GamePhase::Ejection:
			if (settings.taskbarUpdateStyle == 1)
				updateTaskbar();
			resetCooldowns();
			checkForGameOver(timer);
			if (phase == GamePhase::Ejection)
				setPhase(GamePhase::Main, 0);
			break;
		case GamePhase::GameOver:
			restartSetup();
			break;
		default:
			break;
		}
	}

	switch (phase)
	{
	case GamePhase::Setup:
		break;
	case GamePhase::Intro:
		break;
	case GamePhase::Main:
		break;
	case GamePhase::Discussion:
		break;
	case GamePhase::Voting:
		break;
	case GamePhase::EndOfMeeting:
		break;
	case GamePhase::Ejection:
		break;
	case GamePhase::GameOver:
		break;
	default:
		break;
	}

	auto sabotageTasksCopy = sabotageTasks;
	for (auto task : sabotageTasksCopy)
	{
		if (task->timer != 0 && task->timer <= handler.time)
		{
			task->expire();
			sendSabotageTaskUpdate(task, true);
			sabotageTasks.erase(std::find(sabotageTasks.begin(), sabotageTasks.end(), task));
		}
	}

	if (phase != GamePhase::Setup && handler.players.empty())
		restartSetup();
}

void Game::setPhase(GamePhase phase, int64_t timer)
{
	GamePhaseUpdate message;
	message.phase = (uint64_t)phase;
	message.timer = timer;
	message.previous = (uint64_t)this->phase;
	handler.Broadcast(message);
	this->phase = phase;
	this->timer = timer;
}

void Game::teleportPlayersToEllipse(const Vec2& position, const Vec2& size)
{
	std::vector<size_t> mobs;
	for (auto player : handler.players)
	{
		mobs.push_back(player.second.mob);
	}

	for (size_t i = 0; i + 1 < mobs.size(); ++i)
	{
		std::swap(mobs[i], mobs[handler.rng.next(i, mobs.size() - 1)]);
	}

	float offset = handler.rng.next_float() * (float)M_PI * 2.0f;
	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[mobs[i]];

		MobTeleport message;
		message.from = mob.position;
		message.to = position + Vec2(cosf(i * (float)M_PI * 2.0f / mobs.size() + offset), sinf(i * (float)M_PI * 2.0f / mobs.size() + offset)) * size;
		message.time = handler.time;
		message.id = mobs[i];
		handler.Broadcast(message);

		mob.position = message.to;
	}
}

void Game::startGameCountdown()
{
	if (phase == GamePhase::Setup && timer == 0)
	{
		setPhase(GamePhase::Setup, handler.time + 5'000'000'000);
	}
}

void Game::startGame()
{
	setPhase(GamePhase::Intro, timer + 6'000'000'000);
	teleportPlayersToEllipse(map->spawnPos, map->spawnSize);

	std::vector<size_t> mobs;
	for (auto player : handler.players)
	{
		mobs.push_back(player.second.mob);
	}

	for (size_t i = 0; i + 1 < mobs.size(); ++i)
	{
		std::swap(mobs[i], mobs[handler.rng.next(i, mobs.size() - 1)]);
	}

	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[mobs[i]];
		mob.role = i < settings.impostorCount ? Role::Impostor : Role::Crewmate;
		mob.meetingsCalled = 0;
	}

	handler.updateMobRoles();

	createTaskLists();

	updateTaskbar();
}

void Game::startMeeting(uint64_t caller, uint64_t corpse)
{
	if (phase == GamePhase::Main)
	{
		if (corpse == -1ul)
		{
			MeetingRequested message;
			message.idOfInitiator = caller;
			handler.Broadcast(message);
		}
		else
		{
			ReportAttempted message;
			message.idOfInitiator = caller;
			message.target = corpse;
			handler.Broadcast(message);
		}

		if (settings.taskbarUpdateStyle == 2)
			updateTaskbar();
		removeCorpses();
		resetVotes();
		setPhase(GamePhase::Discussion, handler.time + settings.discussionTime);
		teleportPlayersToEllipse(map->meetingPos, map->meetingSize);
	}
}

void Game::endMeeting(int64_t now)
{
	toBeEjected.clear();

	if (settings.showVotesWhenEveryoneHasVoted)
	{
		for (auto vote : votes)
		{
			PlayerVoted reply;
			if (settings.anonymousVotes)
				reply.voter = -1;
			else
				reply.voter = vote.first;
			reply.target = vote.second;
			handler.Broadcast(reply);
		}
	}

	std::vector<size_t> tally(handler.mobs.size() + 1);
	for (auto vote : votes)
		++tally[vote.second + 1];

	if (settings.killOnTies)
	{
		size_t mostVotes = tally[0];
		for (size_t i = 1; i < tally.size(); ++i)
		{
			if (tally[i] > mostVotes)
				mostVotes = tally[i];
		}

		for (size_t i = 1; i < tally.size(); ++i)
		{
			if (tally[i] == mostVotes)
				toBeEjected.push_back(i - 1);
		}
	}
	else
	{
		uint64_t hasMostVotes = 0;
		size_t mostVotes = tally[0];
		for (size_t i = 1; i < tally.size(); ++i)
		{
			if (tally[i] > mostVotes)
			{
				hasMostVotes = i;
				mostVotes = tally[i];
			}
		}

		if (hasMostVotes != 0)
			toBeEjected.push_back(hasMostVotes - 1);
	}

	for (auto i : toBeEjected)
	{
		PlayerVoted message;
		message.voter = -3ul;
		message.target = i;
		handler.Broadcast(message);
	}

	resetVotes();
	setPhase(GamePhase::EndOfMeeting, now + 5'000'000'000);
}

void Game::restartSetup()
{
	fixAllSabotages();
	setPhase(GamePhase::Setup, 0);
	teleportPlayersToEllipse(map->spawnPos, map->spawnSize);
	for (size_t i = 0; i < handler.mobs.size(); ++i)
		handler.removeMob(i);
	for (auto player : handler.players)
	{
		auto&& mob = handler.mobs[player.second.mob];
		mob.enabled = true;
		mob.type = MobType::Player;
		mob.role = Role::Crewmate;
	}
	handler.updateMobStates();
	handler.updateMobRoles();
}

void Game::resetVotes()
{
	votes.clear();
	for (size_t i = 0; i < handler.mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[i];
		if (mob.enabled && mob.type == MobType::Player)
		{
			mob.votesCast = 0;
		}
	}
}

bool Game::callSabotage(uint64_t index)
{
	if (index < map->sabotages.size())
	{
		auto&& sabotage = *map->sabotages[index];
		auto task = sabotage.task.lock();
		if (!task)
		{
			task = sabotage.call(*this, handler.time);
			if (task)
			{
				task->sabotage_index = index;
				sabotage.task = task;
				sabotageTasks.push_back(task);
				sendSabotageTaskUpdate(task, false);
				return true;
			}
		}
	}
	return false;
}

void Game::fixSabotage(uint64_t index)
{
	auto it = std::find_if(sabotageTasks.begin(), sabotageTasks.end(), [index](auto task)
		{
			return task->sabotage_index == index;
		}
	);
	if (it != sabotageTasks.end())
	{
		(*it)->fix();
		sendSabotageTaskUpdate(*it, true);
		sabotageTasks.erase(it);
	}
}

void Game::fixAllSabotages()
{
	for (auto task : sabotageTasks)
		task->fix();
	sabotageTasks.clear();
}

void Game::sendSabotageTaskUpdate(std::shared_ptr<SabotageTask> task, bool completed)
{
	SabotageTaskUpdate message;
	message.sabotage = task->sabotage_index;
	message.completed = completed;
	message.minigame_index = task->minigame_index;
	message.timer = task->timer;
	handler.Broadcast(message);
}

void Game::createTaskLists()
{
	for (auto player : handler.players)
	{
		auto&& mob = handler.mobs[player.second.mob];
		mob.tasks.clear();

		std::vector<uint16_t> tasks;
		for (size_t i = 0; i < 24; ++i)
			tasks.push_back(i);

		for (size_t i = 0; i + 1 < tasks.size(); ++i)
			std::swap(tasks[i], tasks[handler.rng.next(i, tasks.size() - 1)]);

		if (settings.taskCount < tasks.size())
			tasks.resize(settings.taskCount);

		if (mob.role == Role::Crewmate)
			for (auto task : tasks)
				mob.tasks.push_back(Task{ task, false });

		TaskListUpdate message;
		message.tasks = tasks;
		message.password = handler.rng.next();
		message.passwordSuffix = handler.rng.next();
		message.passwordLocation = handler.rng.next();
		handler.link.Send(player.first, message);
	}
}

void Game::resetCooldowns()
{
	{
		KillAttempted message;
		message.target = -1ul;
		message.killer = -1ul;
		message.time = handler.time + settings.killCooldown;
		handler.Broadcast(message);
		for (auto&& mob : handler.mobs)
			mob.killCooldown = message.time;
	}

	{
		AbilityUsed message;
		message.time = handler.time + settings.sabotageCooldown;
		handler.Broadcast(message);
		for (auto&& mob : handler.mobs)
			mob.sabotageCooldown = message.time;
	}
}

void Game::resetSettings()
{
	if (phase == GamePhase::Setup)
	{
		settings.impostorCount = 1;
		settings.votesPerPlayer = 1;
		settings.emergencyMeetingsPerPlayer = 1;
		settings.emergencyMeetingCooldown = 10'000'000'000;
		settings.killCooldown = 15'000'000'000;
		settings.killRange = 2.0f;
		settings.voteTime = 30'000'000'000;
		settings.discussionTime = 30'000'000'000;
		settings.killVictoryEnabled = true;
		settings.crewmateVision = 5.0f;
		settings.impostorVision = 10.0f;
		settings.playerSpeed = 4.0f;
		settings.killOnTies = false;
		settings.enableSkipButton = true;
		settings.showVotesWhenEveryoneHasVoted = true;
		settings.anonymousVotes = false;
		settings.taskCount = 5;
		settings.taskbarUpdateStyle = 2;
		settings.sabotageCooldown = 30'000'000'000;
		settings.gameOverEnabled = true;

		if (handler.players.size() > 8) // 6
		{
			settings.impostorCount = (handler.players.size() - 1) / 4; // 3
			settings.killCooldown = 15'000'000'000 * settings.impostorCount;
			settings.emergencyMeetingCooldown = settings.killCooldown - 5'000'000'000;
			settings.sabotageCooldown = 15'000'000'000 * (settings.impostorCount + 1);
			settings.voteTime = 30'000'000'000 * settings.impostorCount;
			settings.discussionTime = 30'000'000'000 * settings.impostorCount;
		}

		handler.Broadcast(settings);
	}
}

void Game::checkForGameOver(int64_t now)
{
	if (!settings.gameOverEnabled)
		return;

	if (settings.killVictoryEnabled)
	{
		size_t crew = 0;
		size_t impostors = 0;
		for (auto player : handler.players)
		{
			auto&& mob = handler.mobs[player.second.mob];
			if (mob.type == MobType::Player)
			{
				if (mob.role == Role::Crewmate)
					++crew;
				if (mob.role == Role::Impostor)
					++impostors;
			}
		}

		if (impostors >= crew)
		{
			// impostor victory
			GameOver message;
			message.role = Role::Impostor;
			for (auto player : handler.players)
			{
				if (handler.mobs[player.second.mob].role == Role::Impostor)
					message.winners.push_back(player.second.mob);
			}
			handler.Broadcast(message);
			endGame(now);
			return;
		}

		if (impostors == 0)
		{
			// crew victory
			GameOver message;
			message.role = Role::Crewmate;
			for (auto player : handler.players)
			{
				if (handler.mobs[player.second.mob].role == Role::Crewmate)
					message.winners.push_back(player.second.mob);
			}
			handler.Broadcast(message);
			endGame(now);
			return;
		}
	}

	{
		size_t taskCountAll = 0;
		size_t taskCountComplete = 0;
		for (auto player : handler.players)
		{
			auto&& mob = handler.mobs[player.second.mob];
			if (mob.role == Role::Crewmate)
			{
				for (auto task : mob.tasks)
				{
					++taskCountAll;
					if (task.completed)
						++taskCountComplete;
				}
			}
		}
		if (taskCountComplete >= taskCountAll)
		{
			// crew victory
			GameOver message;
			message.role = Role::Crewmate;
			for (auto player : handler.players)
			{
				if (handler.mobs[player.second.mob].role == Role::Crewmate)
					message.winners.push_back(player.second.mob);
			}
			handler.Broadcast(message);
			endGame(now);
			return;
		}
	}
}

void Game::endGame(int64_t now)
{
	setPhase(GamePhase::GameOver, now + 10'000'000'000);
}

void Game::removeCorpses()
{
	for (size_t i = 0; i < handler.mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[i];
		if (mob.enabled && mob.type == MobType::Corpse)
			handler.removeMob(i);
	}
}

void Game::updateTaskbar()
{
	size_t taskCountComplete = 0;
	for (auto player : handler.players)
	{
		auto&& mob = handler.mobs[player.second.mob];
		if (mob.role == Role::Crewmate)
		{
			for (auto task : mob.tasks)
			{
				if (task.completed)
					++taskCountComplete;
			}
		}
	}

	TaskUpdate message;
	message.task = taskCountComplete;
	handler.Broadcast(message);
}

void Game::setLight(float light)
{
	LightUpdate message;
	message.time = handler.time;
	message.light = light;
	handler.Broadcast(message);
}

void Game::setVoice(bool enabled)
{
	voiceEnabled = enabled;
}

void Game::checkForMapChange()
{
	if (settings.map < maps.size())
	{
		if (map != maps[settings.map])
		{
			map = maps[settings.map];
			restartSetup();
		}
	}
}

