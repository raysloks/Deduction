#include "GetAllPlayerPositions.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GetAllPlayerPositions::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->playerPos.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->playerPos.data(), sizeof(Vec3) * size);
	}
	os.write((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	os.write((char*)&this->player, (sizeof(this->player) + 3) / 4 * 4);
}

void GetAllPlayerPositions::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->playerPos.resize(size);
		is.read((char*)this->playerPos.data(), sizeof(Vec3) * size);
	}
	is.read((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	is.read((char*)&this->player, (sizeof(this->player) + 3) / 4 * 4);
}

