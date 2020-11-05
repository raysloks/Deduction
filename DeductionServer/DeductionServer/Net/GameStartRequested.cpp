#include "GameStartRequested.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GameStartRequested::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(GameStartRequested));
}

void GameStartRequested::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(GameStartRequested));
}

