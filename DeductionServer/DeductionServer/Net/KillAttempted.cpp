#include "KillAttempted.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void KillAttempted::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(KillAttempted));
}

void KillAttempted::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(KillAttempted));
}

