#include "MobTeleport.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void MobTeleport::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(MobTeleport));
}

void MobTeleport::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(MobTeleport));
}

