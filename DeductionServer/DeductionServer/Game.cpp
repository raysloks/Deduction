#include "Game.h"

#include "NetworkHandler.h"

Game::Game(NetworkHandler& handler) : handler(handler)
{
	phase = GamePhase::Setup;
	timer = 0;

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
		case GamePhase::Main:
			break;
		case GamePhase::Meeting:
			//handler.Broadcast(message);
			//setPhase(GamePhase::Main, 0);
			teleportPlayersToEllipse(Vec2(), Vec2(1.0f));
			resetKillCooldowns();
			break;
		default:
			break;
		}
	}

	switch (phase)
	{
	case GamePhase::Setup:
		break;
	case GamePhase::Main:
		break;
	case GamePhase::Meeting:
		break;
	default:
		break;
	}

	if (phase != GamePhase::Setup && handler.players.empty())
		restartSetup();
}

void Game::setPhase(GamePhase phase, int64_t timer)
{
	this->phase = phase;
	this->timer = timer;
	GamePhaseUpdate message;
	message.phase = (uint64_t)phase;
	message.timer = timer;
	handler.Broadcast(message);
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

	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[mobs[i]];

		MobTeleport message;
		message.from = mob.position;
		message.to = position + Vec2(cosf(i * (float)M_PI * 2.0f / mobs.size()), sinf(i * (float)M_PI * 2.0f / mobs.size())) * size;
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
		for (auto&& mob : handler.mobs)
		{
			mob.timesVoted = 0;
			mob.meetingsCalled = 0;
		}
	}
}

void Game::startGame()
{
	setPhase(GamePhase::Main, 0);
	teleportPlayersToEllipse(Vec2(), Vec2(1.0f));

	std::vector<size_t> mobs;
	for (auto player : handler.players)
	{
		mobs.push_back(player.second.mob);
	}

	for (size_t i = 0; i < mobs.size() - 1; ++i)
	{
		std::swap(mobs[i], mobs[handler.rng.next(i, mobs.size() - 1)]);
	}

	for (size_t i = 0; i < mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[mobs[i]];
		mob.role = i < settings.impostorCount ? Role::Impostor : Role::Crewmate;
	}

	handler.updateMobRoles();

	resetKillCooldowns();
}

void Game::startMeeting()
{
	if (phase == GamePhase::Main)
	{
		setPhase(GamePhase::Meeting, handler.time + settings.discussionTime + settings.voteTime);
		teleportPlayersToEllipse(Vec2(), Vec2(1.0f));
		removeCorpses();
		resetVotes();
	}
}

void Game::restartSetup()
{
	if (phase != GamePhase::Setup)
	{
		setPhase(GamePhase::Setup, 0);
		teleportPlayersToEllipse(Vec2(), Vec2(1.0f));
		for (size_t i = 0; i < handler.mobs.size(); ++i)
			handler.removeMob(i);
		for (auto player : handler.players)
		{
			auto&& mob = handler.mobs[player.second.mob];
			mob.enabled = true;
			mob.type = MobType::Player;
			mob.role = Role::Crewmate;
		}
		handler.createMob();
		handler.updateMobStates();
		handler.updateMobRoles();
	}
}

void Game::resetVotes()
{
	for (size_t i = 0; i < handler.mobs.size(); ++i)
	{
		auto&& mob = handler.mobs[i];
		if (mob.enabled && mob.type == MobType::Player)
			mob.timesVoted = 0;
	}
}

void Game::resetKillCooldowns()
{
	KillAttempted message;
	message.time = handler.time + settings.killCooldown;
	handler.Broadcast(message);
	for (auto&& mob : handler.mobs)
		mob.killCooldown = message.time;
}

void Game::resetSettings()
{
	if (phase == GamePhase::Setup)
	{
		settings.impostorCount = 1;
		settings.votesPerPlayer = 1;
		settings.emergencyMeetingsPerPlayer = 1;
		settings.emergencyMeetingCooldown = 15'000'000'000;
		settings.killCooldown = 30'000'000'000;
		settings.voteTime = 30'000'000'000;
		settings.discussionTime = 90'000'000'000;
		settings.killVictoryEnabled = false;
		settings.crewmateVision = 5.0f;
		settings.impostorVision = 10.0f;
		settings.playerSpeed = 4.0f;
		settings.killOnTies = false;
		settings.enableSkipButton = true;
		settings.showVotesWhenEveryoneHasVoted = false;

		handler.Broadcast(settings);
	}
}

void Game::checkForGameOver()
{
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
		}

		if (impostors == 0)
		{
			// crew victory
		}
	}
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
