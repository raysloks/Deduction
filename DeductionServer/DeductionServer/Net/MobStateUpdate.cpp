#include "MobStateUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void MobStateUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(MobStateUpdate));
}

void MobStateUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(MobStateUpdate));
}

