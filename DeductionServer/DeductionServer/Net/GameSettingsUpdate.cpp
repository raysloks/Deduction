#include "GameSettingsUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GameSettingsUpdate::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->values.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->values.data(), sizeof(int64_t) * size);
	}
}

void GameSettingsUpdate::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->values.resize(size);
		is.read((char*)this->values.data(), sizeof(int64_t) * size);
	}
}

