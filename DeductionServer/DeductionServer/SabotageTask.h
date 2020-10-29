#pragma once

#include <functional>

class SabotageTask
{
public:
	~SabotageTask();

	void fix();
	void expire();

	std::function<void(void)> on_fixed, on_expired, on_done;

	int64_t sabotage_index;
	int64_t minigame_index;
	int64_t timer;
};

