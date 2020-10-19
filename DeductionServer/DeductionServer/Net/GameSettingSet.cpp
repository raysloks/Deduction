#include "GameSettingSet.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void GameSettingSet::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(GameSettingSet));
}

void GameSettingSet::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(GameSettingSet));
}

