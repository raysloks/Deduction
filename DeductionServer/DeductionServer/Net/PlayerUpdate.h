#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <string>
#include <iostream>

#pragma pack(push, 1)
class PlayerUpdate
{
public:
	uint64_t id;
	uint64_t mob;
	std::string name;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
