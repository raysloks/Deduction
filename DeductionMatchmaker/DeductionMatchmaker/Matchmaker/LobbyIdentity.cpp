#include "LobbyIdentity.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void LobbyIdentity::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->lobby.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->lobby.data(), size);
	}
	{
		uint16_t size = this->address.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->address.data(), size);
	}
	os.write((char*)&this->port, (sizeof(this->port) + 3) / 4 * 4);
}

void LobbyIdentity::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->lobby.resize(size);
		is.read((char*)this->lobby.data(), size);
	}
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->address.resize(size);
		is.read((char*)this->address.data(), size);
	}
	is.read((char*)&this->port, (sizeof(this->port) + 3) / 4 * 4);
}

