#pragma once

#include <vector>
#include <memory>

#include "Sabotage.h"
#include "Vec2.h"

class Coal;

class Map
{
public:
	Map();
	Map(const Coal& coal);

	std::vector<std::unique_ptr<Sabotage>> sabotages;

	Vec2 loungePos, loungeSize;
	Vec2 spawnPos, spawnSize;
	Vec2 meetingPos, meetingSize;

	uint64_t shortTaskCount, longTaskCount;
};

