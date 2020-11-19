#include "SendEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "PhotoPose.h"

void SendEvidence::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->poses.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->poses.data(), sizeof(PhotoPose) * size);
	}
	os.write((char*)&photographer, (sizeof(photographer) + 3) / 4 * 4);
	os.write((char*)&index, (sizeof(index) + 3) / 4 * 4);
}

void SendEvidence::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->poses.resize(size);
		is.read((char*)this->poses.data(), sizeof(PhotoPose) * size);
	}
	is.read((char*)&photographer, (sizeof(photographer) + 3) / 4 * 4);
	is.read((char*)&index, (sizeof(index) + 3) / 4 * 4);
}

