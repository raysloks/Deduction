#include "TaskUpdate.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void TaskUpdate::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(TaskUpdate));
}

void TaskUpdate::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(TaskUpdate));
}

