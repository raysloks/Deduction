#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

#include "../Vec3.h"

#pragma pack(push, 1)
class GetAllPlayerPositions
{
public:
	std::vector<Vec3> playerPos;
	uint64_t id;
	Vec3 player;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
