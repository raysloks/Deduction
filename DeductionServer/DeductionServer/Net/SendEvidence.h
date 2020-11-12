#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

#include "../Vec3.h"

#pragma pack(push, 1)
class SendEvidence
{
public:
	std::vector<Vec3> picturePos;
	uint64_t id;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
