#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "../Vec3.h"

#pragma pack(push, 1)
class PhotoPose
{
public:
	uint64_t index;
	Vec3 position;
	bool flipped;
	bool moving;
	bool dead;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)