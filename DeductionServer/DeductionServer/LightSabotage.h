#pragma once

#include "Sabotage.h"

class LightSabotage :
	public Sabotage
{
public:
	LightSabotage();
	~LightSabotage();

	void call(Game& game, int64_t now);
};
