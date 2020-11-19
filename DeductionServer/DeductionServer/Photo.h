#pragma once

#include <vector>

#include "Net/PhotoPose.h"

class Photo
{
public:
	std::vector<PhotoPose> poses;
	uint64_t photographer;
};

