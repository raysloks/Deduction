#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <string>
#include <iostream>

#pragma pack(push, 1)
class GameStartRequested
{
public:
	std::string password;
	uint64_t passwordLocation;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
