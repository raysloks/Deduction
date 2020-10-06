#include "Heartbeat.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void Heartbeat::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(Heartbeat));
}

void Heartbeat::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(Heartbeat));
}

