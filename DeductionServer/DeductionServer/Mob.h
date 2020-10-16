#pragma once

#include <vector>

#include "Vec3.h"

#include "MobType.h"
#include "Role.h"

#include "Task.h"

#include <cstdint>

class Mob
{
public:
	Mob();

	bool visibleTo(const Mob& mob) const;

	Vec3 position;
	MobType type;
	Role role;
	int timesVoted;
	int totalVotes;
	bool enabled;

	std::vector<Task> tasks;

	int64_t killCooldown;

	Vec3 color;
};
