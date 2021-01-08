#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <string>
#include <iostream>

#pragma pack(push, 1)
class SmokeGrenadeEvidence
{
public:
	std::string area;
	std::string playerName;
	uint64_t playerId;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
