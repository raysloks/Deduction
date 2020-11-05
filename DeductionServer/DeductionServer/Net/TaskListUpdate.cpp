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
	os.write((char*)&password, (sizeof(password) + 3) / 4 * 4);
	os.write((char*)&passwordSuffix, (sizeof(passwordSuffix) + 3) / 4 * 4);
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
	is.read((char*)&password, (sizeof(password) + 3) / 4 * 4);
	is.read((char*)&passwordSuffix, (sizeof(passwordSuffix) + 3) / 4 * 4);
	is.read((char*)&passwordLocation, (sizeof(passwordLocation) + 3) / 4 * 4);
}

