#include "SendEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SendEvidence::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->picturePos.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->picturePos.data(), sizeof(Vec3) * size);
	}
	os.write((char*)&id, (sizeof(id) + 3) / 4 * 4);
}

void SendEvidence::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->picturePos.resize(size);
		is.read((char*)this->picturePos.data(), sizeof(Vec3) * size);
	}
	is.read((char*)&id, (sizeof(id) + 3) / 4 * 4);
}

