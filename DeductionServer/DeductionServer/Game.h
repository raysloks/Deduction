#pragma once

#include <cstdint>

#include "GamePhase.h"
#include "Vec2.h"
#include "GameSettings.h"

class NetworkHandler;

class Game
{
public:
	Game(NetworkHandler& handler);

	void tick(int64_t now);

	void setPhase(GamePhase phase, int64_t timer);

	void teleportPlayersToEllipse(const Vec2& position, const Vec2& size);

	void startGameCountdown();
	void startGame();
	void startMeeting();
	void restartSetup();
	void resetVotes();

	void resetKillCooldowns();

	void checkForGameOver();

	void removeCorpses();

	GamePhase phase;

	int64_t timer;

	NetworkHandler& handler;

	GameSettings settings;
};

