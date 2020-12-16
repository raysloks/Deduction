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
	os.write((char*)&this->password, (sizeof(this->password) + 3) / 4 * 4);
	os.write((char*)&this->passwordSuffix, (sizeof(this->passwordSuffix) + 3) / 4 * 4);
	os.write((char*)&this->passwordLocation, (sizeof(this->passwordLocation) + 3) / 4 * 4);
}

void TaskListUpdate::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->tasks.resize(size);
		is.read((char*)this->tasks.data(), sizeof(uint16_t) * size);
	}
	is.read((char*)&this->password, (sizeof(this->password) + 3) / 4 * 4);
	is.read((char*)&this->passwordSuffix, (sizeof(this->passwordSuffix) + 3) / 4 * 4);
	is.read((char*)&this->passwordLocation, (sizeof(this->passwordLocation) + 3) / 4 * 4);
}

