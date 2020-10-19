#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "MobUpdate.h"
#include "../Vec3.h"

#pragma pack(push, 1)
class MobStateUpdate
{
public:
	MobUpdate update;
	uint64_t type;
	uint64_t state;
	uint64_t sprite;
	Vec3 color;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
