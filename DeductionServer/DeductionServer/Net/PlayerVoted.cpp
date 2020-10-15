#include "PlayerVoted.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PlayerVoted::serialize(std::ostream& os) const
{
	os.write((char*)&phase, (sizeof(phase) + 3) / 4 * 4);
	os.write((char*)&timer, (sizeof(timer) + 3) / 4 * 4);
	os.write((char*)&id, (sizeof(id) + 3) / 4 * 4);
	os.write((char*)&totalVotes, (sizeof(totalVotes) + 3) / 4 * 4);
	{
		uint16_t size = this->buttonName.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->buttonName.data(), size);
	}
	os.write((char*)&votesLeft, (sizeof(votesLeft) + 3) / 4 * 4);
}

void PlayerVoted::deserialize(std::istream& is)
{
	is.read((char*)&phase, (sizeof(phase) + 3) / 4 * 4);
	is.read((char*)&timer, (sizeof(timer) + 3) / 4 * 4);
	is.read((char*)&id, (sizeof(id) + 3) / 4 * 4);
	is.read((char*)&totalVotes, (sizeof(totalVotes) + 3) / 4 * 4);
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->buttonName.resize(size);
		is.read((char*)this->buttonName.data(), size);
	}
	is.read((char*)&votesLeft, (sizeof(votesLeft) + 3) / 4 * 4);
}

