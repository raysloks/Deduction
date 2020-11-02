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
	{
		uint16_t size = this->password.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->password.data(), size);
	}
	os.write((char*)&passwordLocation, (sizeof(passwordLocation) + 3) / 4 * 4);
}

void TaskListUpdate::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->tasks.resize(size);
		is.read((char*)this->tasks.data(), sizeof(uint16_t) * size);
	}
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->password.resize(size);
		is.read((char*)this->password.data(), size);
	}
	is.read((char*)&passwordLocation, (sizeof(passwordLocation) + 3) / 4 * 4);
}

