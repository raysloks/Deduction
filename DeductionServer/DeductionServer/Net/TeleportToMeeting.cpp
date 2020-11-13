#include "TeleportToMeeting.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void TeleportToMeeting::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(TeleportToMeeting));
}

void TeleportToMeeting::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(TeleportToMeeting));
}

