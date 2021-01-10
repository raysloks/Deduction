#include "SendSensorList.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SendSensorList::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->names.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->names.data(), sizeof(uint8_t) * size);
	}
	{
		uint16_t size = this->times.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->times.data(), sizeof(int32_t) * size);
	}
	os.write((char*)&this->player, (sizeof(this->player) + 3) / 4 * 4);
	{
		uint16_t size = this->playerIds.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->playerIds.data(), sizeof(uint64_t) * size);
	}
	os.write((char*)&this->totalRoundTime, (sizeof(this->totalRoundTime) + 3) / 4 * 4);
}

void SendSensorList::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->names.resize(size);
		is.read((char*)this->names.data(), sizeof(uint8_t) * size);
	}
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->times.resize(size);
		is.read((char*)this->times.data(), sizeof(int32_t) * size);
	}
	is.read((char*)&this->player, (sizeof(this->player) + 3) / 4 * 4);
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->playerIds.resize(size);
		is.read((char*)this->playerIds.data(), sizeof(uint64_t) * size);
	}
	is.read((char*)&this->totalRoundTime, (sizeof(this->totalRoundTime) + 3) / 4 * 4);
}

