#include "PresentEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PresentEvidence::serialize(std::ostream& os) const
{
	os.write((char*)&index, (sizeof(index) + 3) / 4 * 4);
	os.write((char*)&presenter, (sizeof(presenter) + 3) / 4 * 4);
	{
		uint16_t size = this->sensornames.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->sensornames.data(), sizeof(uint8_t) * size);
	}
}

void PresentEvidence::deserialize(std::istream& is)
{
	is.read((char*)&index, (sizeof(index) + 3) / 4 * 4);
	is.read((char*)&presenter, (sizeof(presenter) + 3) / 4 * 4);
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->sensornames.resize(size);
		is.read((char*)this->sensornames.data(), sizeof(uint8_t) * size);
	}
}

