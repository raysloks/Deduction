#pragma once

#include "Sabotage.h"

class LightSabotage :
	public Sabotage
{
public:
	LightSabotage();
	~LightSabotage();

	std::shared_ptr<SabotageTask> call(Game& game, int64_t now);

	int64_t minigame_index;
	int64_t duration;
};
