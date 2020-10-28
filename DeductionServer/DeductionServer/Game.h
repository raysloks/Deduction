#pragma once

#include <cstdint>
#include <vector>
#include <map>
#include <functional>

#include "GamePhase.h"
#include "Vec2.h"
#include "Net/GameSettings.h"
#include "Map.h"

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
	void restartSetup();

	void startMeeting(uint64_t caller, uint64_t corpse = -1ul);
	void endMeeting(int64_t now);
	void resetVotes();

	void createTaskLists();

	void resetCooldowns();

	void resetSettings();

	void checkForGameOver();
	void endGame();

	void removeCorpses();

	GamePhase phase;

	int64_t timer;

	NetworkHandler& handler;

	GameSettings settings;

	std::vector<std::pair<uint64_t, uint64_t>> votes;

	std::vector<uint64_t> toBeEjected;

	std::multimap<int64_t, std::function<void(void)>> timers;

	std::shared_ptr<Map> map;
};

