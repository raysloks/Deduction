#pragma once

#include <cstdint>
#include <vector>

#include "Vec3.h"

#include "MobType.h"
#include "Role.h"

#include "Task.h"

class Mob
{
public:
	Mob();

	bool visibleTo(const Mob& mob) const;

	bool enabled;
	Vec3 position;
	int64_t time;
	MobType type;
	Role role;
	uint64_t sprite;

	int votesCast;
	int meetingsCalled;

	std::vector<Task> tasks;

	int64_t killCooldown;
	int64_t sabotageCooldown;

	Vec3 color;

	bool flipped;
};

