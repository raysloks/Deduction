#include "SmokeGrenadeEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SmokeGrenadeEvidence::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->area.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->area.data(), size);
	}
	{
		uint16_t size = this->playerName.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->playerName.data(), size);
	}
	os.write((char*)&this->playerId, (sizeof(this->playerId) + 3) / 4 * 4);
}

void SmokeGrenadeEvidence::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->area.resize(size);
		is.read((char*)this->area.data(), size);
	}
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->playerName.resize(size);
		is.read((char*)this->playerName.data(), size);
	}
	is.read((char*)&this->playerId, (sizeof(this->playerId) + 3) / 4 * 4);
}

