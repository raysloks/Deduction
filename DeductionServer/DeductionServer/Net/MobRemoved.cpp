#include "MobRemoved.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void MobRemoved::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(MobRemoved));
}

void MobRemoved::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(MobRemoved));
}

