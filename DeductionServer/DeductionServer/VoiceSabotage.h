#pragma once

#include "Sabotage.h"

class VoiceSabotage :
	public Sabotage
{
public:
	VoiceSabotage();
	~VoiceSabotage();

	std::shared_ptr<SabotageTask> call(Game& game, int64_t now);

	int64_t minigame_index;
	int64_t duration;
};

