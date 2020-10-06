#include "Mob.h"

Mob::Mob()
{
	type = MobType::Player;
	role = Role::Crewmate;
}

bool Mob::visibleTo(const Mob& mob) const
{
	return type != MobType::Ghost || mob.type == MobType::Ghost;
}
