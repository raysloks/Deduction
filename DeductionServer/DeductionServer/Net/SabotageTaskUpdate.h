#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#pragma pack(push, 1)
class SabotageTaskUpdate
{
public:
	uint16_t sabotage;
	uint16_t minigame_index;
	int64_t timer;
	bool completed;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
