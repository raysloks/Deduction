#include "GameSettings.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GameSettings::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(GameSettings));
}

void GameSettings::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(GameSettings));
}

