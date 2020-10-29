#include "LightSabotage.h"

#include "Game.h"
#include "SabotageTask.h"

LightSabotage::LightSabotage()
{
}

LightSabotage::~LightSabotage()
{
}

std::shared_ptr<SabotageTask> LightSabotage::call(Game & game, int64_t now)
{
	game.setLight(0.0f);
	auto task = std::make_shared<SabotageTask>();
	task->on_done = [&game]()
	{
		game.setLight(1.0f);
	};
	task->minigame_index = minigame_index;
	if (duration != 0)
		task->timer = now + duration;
	return task;
}
