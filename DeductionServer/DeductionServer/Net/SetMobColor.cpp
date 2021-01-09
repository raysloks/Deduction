#include "SetMobColor.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SetMobColor::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(SetMobColor));
}

void SetMobColor::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(SetMobColor));
}

