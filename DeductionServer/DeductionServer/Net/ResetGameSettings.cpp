#include "ResetGameSettings.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void ResetGameSettings::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(ResetGameSettings));
}

void ResetGameSettings::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(ResetGameSettings));
}

