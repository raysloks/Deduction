#include "TakePhoto.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void TakePhoto::serialize(std::ostream& os) const
{
	os.write((char*)this, sizeof(TakePhoto));
}

void TakePhoto::deserialize(std::istream& is)
{
	is.read((char*)this, sizeof(TakePhoto));
}

