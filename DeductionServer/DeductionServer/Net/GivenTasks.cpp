#include "GivenTasks.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GivenTasks::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->taskId.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->taskId.data(), sizeof(uint8_t) * size);
	}
}

void GivenTasks::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->taskId.resize(size);
		is.read((char*)this->taskId.data(), sizeof(uint8_t) * size);
	}
}

