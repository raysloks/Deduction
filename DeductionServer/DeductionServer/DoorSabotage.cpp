#include "DoorSabotage.h"

#include "NetworkHandler.h"
#include "SabotageTask.h"

DoorSabotage::DoorSabotage()
{
}

DoorSabotage::~DoorSabotage()
{
}

std::shared_ptr<SabotageTask> DoorSabotage::call(Game & game, int64_t now)
{
	for (auto door : doors)
	{
		DoorUpdate message;
		message.door = door;
		message.open = false;
		game.handler.Broadcast(message);
	}

	auto task = std::make_shared<SabotageTask>();
	task->on_done = [&game, this]()
	{
		for (auto door : doors)
		{
			DoorUpdate message;
			message.door = door;
			message.open = true;
			game.handler.Broadcast(message);
		}
	};
	task->minigame_index = minigame_index;
	if (duration != 0)
		task->timer = now + duration;
	return task;
}
