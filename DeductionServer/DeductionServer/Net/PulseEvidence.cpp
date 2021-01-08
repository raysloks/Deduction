#include "PulseEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PulseEvidence::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(PulseEvidence));
}

void PulseEvidence::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(PulseEvidence));
}

