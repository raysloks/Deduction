#include "DoorSabotage.h"

#include "NetworkHandler.h"

DoorSabotage::DoorSabotage()
{
}

DoorSabotage::~DoorSabotage()
{
}

void DoorSabotage::call(Game & game, int64_t now)
{
	for (auto door : doors)
	{
		DoorUpdate message;
		message.door = door;
		message.open = false;
		game.handler.Broadcast(message);
	}

	game.timers.insert(std::make_pair(now, [this, game]()
		{
			for (auto door : doors)
			{
				DoorUpdate message;
				message.door = door;
				message.open = true;
				game.handler.Broadcast(message);
			}
		}
	));
}
