#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

class PhotoPose;

#pragma pack(push, 1)
class PhotoTaken
{
public:
	std::vector<PhotoPose> poses;
	int32_t photographer;
	uint64_t index;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
