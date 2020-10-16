#include "Mob.h"

Mob::Mob()
{
	type = MobType::Player;
	role = Role::Crewmate;
	killCooldown = 0;
	color = Vec3(0.8f, 0.2f, 0.2f);
}

bool Mob::visibleTo(const Mob& mob) const
{
	return type != MobType::Ghost || mob.type == MobType::Ghost;
}
