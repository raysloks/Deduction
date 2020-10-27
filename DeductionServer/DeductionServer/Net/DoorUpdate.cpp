#include "DoorUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void DoorUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(DoorUpdate));
}

void DoorUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(DoorUpdate));
}

