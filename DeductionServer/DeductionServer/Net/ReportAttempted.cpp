#include "ReportAttempted.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void ReportAttempted::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(ReportAttempted));
}

void ReportAttempted::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(ReportAttempted));
}

