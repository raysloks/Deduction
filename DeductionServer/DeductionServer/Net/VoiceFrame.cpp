#include "VoiceFrame.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

void VoiceFrame::serialize(std::ostream& os) const
{
	os.write((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	{
		uint16_t size = this->data.size();
		os.write((char*)&size, sizeof(size));
		os.write((char*)this->data.data(), sizeof(uint8_t) * size);
	}
}

void VoiceFrame::deserialize(std::istream& is)
{
	is.read((char*)&this->id, (sizeof(this->id) + 3) / 4 * 4);
	{
		uint16_t size;
		is.read((char*)&size, sizeof(size));
		this->data.resize(size);
		is.read((char*)this->data.data(), sizeof(uint8_t) * size);
	}
}

