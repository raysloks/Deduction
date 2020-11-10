#include "SendEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SendEvidence::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->picture.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->picture.data(), sizeof(uint8_t) * size);
	}
}

void SendEvidence::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->picture.resize(size);
		is.read((char*)this->picture.data(), sizeof(uint8_t) * size);
	}
}

