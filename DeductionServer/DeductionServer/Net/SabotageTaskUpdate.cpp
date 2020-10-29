#include "SabotageTaskUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SabotageTaskUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(SabotageTaskUpdate));
}

void SabotageTaskUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(SabotageTaskUpdate));
}

