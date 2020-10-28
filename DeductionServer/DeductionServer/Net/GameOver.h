#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <vector>
#include <iostream>

#pragma pack(push, 1)
class GameOver
{
public:
	std::vector<uint64_t> winners;
	uint64_t role;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
