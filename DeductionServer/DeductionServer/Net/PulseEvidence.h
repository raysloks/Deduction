#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#pragma pack(push, 1)
class PulseEvidence
{
public:
	int32_t deadTime;
	uint64_t playerId;
	uint64_t deadId;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
