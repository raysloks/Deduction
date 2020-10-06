#pragma once

#include "Vec3.h"

#include "MobType.h"
#include "Role.h"

class Mob
{
public:
	Mob();

	bool visibleTo(const Mob& mob) const;

	Vec3 position;
	MobType type;
	Role role;
	bool enabled;
};

