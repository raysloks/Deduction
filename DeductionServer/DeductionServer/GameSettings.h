#pragma once

#include <cstdint>

class GameSettings
{
public:
	union
	{
		int64_t settings[64];
		struct
		{
			int64_t impostorCount = 1;
			int64_t killCooldown = 30'000'000'000;
			int64_t voteTime = 30'000'000'000;
			int64_t discussionTime = 90'000'000'000;
		};
	};
};

