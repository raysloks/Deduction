#include "LightUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void LightUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(LightUpdate));
}

void LightUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(LightUpdate));
}

