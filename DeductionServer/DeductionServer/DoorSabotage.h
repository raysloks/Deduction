#pragma once

#include "Sabotage.h"

#include <vector>

class DoorSabotage :
	public Sabotage
{
public:
	DoorSabotage();
	~DoorSabotage();

	std::shared_ptr<SabotageTask> call(Game& game, int64_t now);

	std::vector<uint16_t> doors;

	int64_t minigame_index;
	int64_t duration;
};

