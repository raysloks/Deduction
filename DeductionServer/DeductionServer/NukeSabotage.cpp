#include "NukeSabotage.h"

#include "SabotageTask.h"
#include "Game.h"

NukeSabotage::NukeSabotage()
{
}

NukeSabotage::~NukeSabotage()
{
}

std::shared_ptr<SabotageTask> NukeSabotage::call(Game & game, int64_t now)
{
	auto task = std::make_shared<SabotageTask>();
	std::weak_ptr<SabotageTask> task_weak = task;
	task->on_expired = [task_weak, &game]()
	{
		game.endGame(task_weak.lock()->timer, Role::Impostor);
	};
	task->minigame_index = minigame_index;
	if (duration != 0)
		task->timer = now + duration;
	return task;
}
