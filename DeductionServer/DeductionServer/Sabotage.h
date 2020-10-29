#pragma once

#include <cstdint>
#include <memory>

#include "SabotageTask.h"

class Game;

class Sabotage
{
public:
	virtual ~Sabotage() {};

	virtual void call(Game& game, int64_t now) = 0;

	std::weak_ptr<SabotageTask> task;
};

