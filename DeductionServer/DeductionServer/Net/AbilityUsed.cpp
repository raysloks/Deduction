#include "AbilityUsed.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void AbilityUsed::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(AbilityUsed));
}

void AbilityUsed::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(AbilityUsed));
}

