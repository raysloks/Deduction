#include "PickupCooldown.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PickupCooldown::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(PickupCooldown));
}

void PickupCooldown::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(PickupCooldown));
}

