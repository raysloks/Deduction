#include "PresentEvidence.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void PresentEvidence::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(PresentEvidence));
}

void PresentEvidence::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(PresentEvidence));
}

