#pragma once

#include <vector>
#include <memory>

#include "Sabotage.h"
#include "Vec2.h"

class Map
{
public:
	Map();

	std::vector<std::unique_ptr<Sabotage>> sabotages;

	Vec2 spawnPos, spawnSize, meetingPos, meetingSize;
};

