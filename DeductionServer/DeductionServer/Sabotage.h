#pragma once

#include <cstdint>

class Game;

class Sabotage
{
public:
	virtual ~Sabotage() {};

	virtual void call(Game& game, int64_t now) = 0;
};

