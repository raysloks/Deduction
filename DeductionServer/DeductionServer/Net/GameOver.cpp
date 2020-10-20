#include "GameOver.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GameOver::serialize(std::ostream& os) const
{
	{
		uint16_t size = this->winners.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->winners.data(), sizeof(uint64_t) * size);
	}
}

void GameOver::deserialize(std::istream& is)
{
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->winners.resize(size);
		is.read((char*)this->winners.data(), sizeof(uint64_t) * size);
	}
}

