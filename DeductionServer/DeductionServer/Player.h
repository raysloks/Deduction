#pragma once

#include <cstdint>
#include <string>

class Player
{
public:
	uint64_t index;
	uint64_t mob;
	std::string name;
	int64_t timeout;
};

