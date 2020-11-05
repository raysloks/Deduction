#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

#pragma pack(push, 1)
class TaskListUpdate
{
public:
	std::vector<uint16_t> tasks;
	uint64_t password;
	uint64_t passwordSuffix;
	uint64_t passwordLocation;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
