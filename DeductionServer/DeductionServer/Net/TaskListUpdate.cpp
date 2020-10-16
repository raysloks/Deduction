#include "TaskListUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void TaskListUpdate::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->tasks.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->tasks.data(), sizeof(uint16_t) * size);
	}
}

void TaskListUpdate::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->tasks.resize(size);
		is.read((char*)this->tasks.data(), sizeof(uint16_t) * size);
	}
}

