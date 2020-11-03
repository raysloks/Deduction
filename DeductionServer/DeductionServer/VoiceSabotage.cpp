#include "VoiceSabotage.h"

#include "Game.h"
#include "SabotageTask.h"

VoiceSabotage::VoiceSabotage()
{
}

VoiceSabotage::~VoiceSabotage()
{
}

std::shared_ptr<SabotageTask> VoiceSabotage::call(Game & game, int64_t now)
{
	game.setVoice(false);
	auto task = std::make_shared<SabotageTask>();
	task->on_done = [&game]()
	{
		game.setVoice(true);
	};
	task->minigame_index = minigame_index;
	if (duration != 0)
		task->timer = now + duration;
	return task;
}
