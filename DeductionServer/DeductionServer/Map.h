#pragma once

#include <vector>
#include <memory>

#include "Vec2.h"

class Sabotage;

class Map
{
public:
	Map();

	std::vector<std::unique_ptr<Sabotage>> sabotages;
};

