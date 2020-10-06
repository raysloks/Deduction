#include "RestartRequested.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void RestartRequested::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(RestartRequested));
}

void RestartRequested::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(RestartRequested));
}

