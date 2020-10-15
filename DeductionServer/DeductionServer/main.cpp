#include <iostream>
#include <chrono>
#include <thread>

#include <sys/random.h>

#include "NetworkHandler.h"

int main()
{
	std::cout << "starting..." << std::endl;

	NetworkHandler handler;

	uint64_t seed[2];
	getrandom((void*)seed, sizeof(seed), 0);
	handler.rng.seed(seed[0], seed[1]);

	std::cout << "started." << std::endl;

	uint64_t tick = 0;

	auto now = std::chrono::steady_clock::now();

	while (true)
	{
		now += std::chrono::milliseconds(50);
		std::this_thread::sleep_until(now);

		handler.tick(now);

		++tick;
	}

	return 0;
}