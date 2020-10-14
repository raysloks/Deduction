#include "LobbyRequest.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void LobbyRequest::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->lobby.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->lobby.data(), size);
	}
}

void LobbyRequest::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->lobby.resize(size);
		is.read((char*)this->lobby.data(), size);
	}
}

