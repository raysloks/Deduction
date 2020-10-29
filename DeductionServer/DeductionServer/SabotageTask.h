#pragma once

#include <functional>

class SabotageTask
{
public:
	~SabotageTask();

	std::function<void(void)> on_fixed, on_expired, on_done;
};

