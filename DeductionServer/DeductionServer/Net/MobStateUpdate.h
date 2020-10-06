#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "MobUpdate.h"

#pragma pack(push, 1)
class MobStateUpdate
{
public:
	MobUpdate update;
	uint64_t type;
	uint64_t state;
	uint64_t sprite;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
