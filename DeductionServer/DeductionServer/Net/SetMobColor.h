#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "../Vec3.h"

#pragma pack(push, 1)
class SetMobColor
{
public:
	Vec3 color;
	int32_t id;
	int32_t former;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
