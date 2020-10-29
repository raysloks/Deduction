#pragma once

#include <cstdint>
#include <memory>

class SabotageTask;
class Game;

class Sabotage
{
public:
	virtual ~Sabotage() {};

	virtual std::shared_ptr<SabotageTask> call(Game& game, int64_t now) = 0;

	std::weak_ptr<SabotageTask> task;
};

