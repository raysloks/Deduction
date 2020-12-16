#include "PlayerUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PlayerUpdate::serialize(std::ostream& os) const
{
	os.write((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	os.write((char*)&this->mob, (sizeof(this->mob) + 3) / 4 * 4);
	{
		uint16_t size = this->name.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->name.data(), size);
	}
}

void PlayerUpdate::deserialize(std::istream& is)
{
	is.read((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	is.read((char*)&this->mob, (sizeof(this->mob) + 3) / 4 * 4);
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->name.resize(size);
		is.read((char*)this->name.data(), size);
	}
}

