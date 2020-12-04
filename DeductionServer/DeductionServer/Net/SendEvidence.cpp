#include "SendEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void SendEvidence::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(SendEvidence));
}

void SendEvidence::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(SendEvidence));
}

