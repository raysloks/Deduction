#include "MobUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void MobUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(MobUpdate));
}

void MobUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(MobUpdate));
}

