#include "PlayerVoted.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PlayerVoted::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(PlayerVoted));
}

void PlayerVoted::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(PlayerVoted));
}

