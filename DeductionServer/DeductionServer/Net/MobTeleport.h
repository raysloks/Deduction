#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "../Vec3.h"

#pragma pack(push, 1)
class MobTeleport
{
public:
	uint64_t id;
	int64_t time;
	Vec3 from;
	Vec3 to;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
