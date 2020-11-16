#include "HideAttempted.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void HideAttempted::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(HideAttempted));
}

void HideAttempted::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(HideAttempted));
}

