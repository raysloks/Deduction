#pragma once

#include "Sabotage.h"

#include <vector>

class DoorSabotage :
	public Sabotage
{
public:
	DoorSabotage();
	~DoorSabotage();

	void call(Game& game, int64_t now);

	std::vector<uint16_t> doors;
};

