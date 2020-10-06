#include "GamePhaseUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GamePhaseUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(GamePhaseUpdate));
}

void GamePhaseUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(GamePhaseUpdate));
}

