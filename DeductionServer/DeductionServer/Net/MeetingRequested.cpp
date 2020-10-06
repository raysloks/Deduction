#include "MeetingRequested.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void MeetingRequested::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(MeetingRequested));
}

void MeetingRequested::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(MeetingRequested));
}

