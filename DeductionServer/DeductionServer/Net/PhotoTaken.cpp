#include "PhotoTaken.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#include "PhotoPose.h"

void PhotoTaken::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->poses.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->poses.data(), sizeof(PhotoPose) * size);
	}
	os.write((char*)&this->photographer, (sizeof(this->photographer) + 3) / 4 * 4);
	os.write((char*)&this->index, (sizeof(this->index) + 3) / 4 * 4);
}

void PhotoTaken::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->poses.resize(size);
		is.read((char*)this->poses.data(), sizeof(PhotoPose) * size);
	}
	is.read((char*)&this->photographer, (sizeof(this->photographer) + 3) / 4 * 4);
	is.read((char*)&this->index, (sizeof(this->index) + 3) / 4 * 4);
}

