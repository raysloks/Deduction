#include "Mob.h"

Mob::Mob()
{
	type = MobType::Player;
	role = Role::Crewmate;
	killCooldown = 0;
	sabotageCooldown = 0;
	color = Vec3(0.8f, 0.2f, 0.2f);
	time = 0;
	votesCast = 0;
	meetingsCalled = 0;
}

bool Mob::visibleTo(const Mob& mob) const
{
	return type != MobType::Ghost || mob.type == MobType::Ghost;
}
