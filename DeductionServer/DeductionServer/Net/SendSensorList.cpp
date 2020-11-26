#include "SendSensorList.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SendSensorList::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->names.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->names.data(), sizeof(std::string) * size);
	}
}

void SendSensorList::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->names.resize(size);
		is.read((char*)this->names.data(), sizeof(std::string) * size);
	}
}

