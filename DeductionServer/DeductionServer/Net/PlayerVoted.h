#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#pragma pack(push, 1)
class PlayerVoted
{
public:
	uint64_t phase;
	int64_t timer;
	uint64_t id;
	int64_t totalVotes;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
