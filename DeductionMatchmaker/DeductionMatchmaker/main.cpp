#include <sys/random.h>

#include "MatchmakerHandler.h"

int main()
{
	MatchmakerHandler handler;

	uint64_t seed[2];
	getrandom((void*)seed, sizeof(seed), 0);
	handler.rng.seed(seed[0], seed[1]);

	for (;;) pause();
}