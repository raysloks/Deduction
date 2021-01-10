#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

#pragma pack(push, 1)
class SendSensorList
{
public:
	std::vector<uint8_t> names;
	std::vector<int32_t> times;
	uint64_t player;
	std::vector<uint64_t> playerIds;
	int32_t totalRoundTime;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
