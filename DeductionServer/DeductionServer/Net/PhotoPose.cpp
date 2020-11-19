#include "PhotoPose.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PhotoPose::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(PhotoPose));
}

void PhotoPose::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(PhotoPose));
}

