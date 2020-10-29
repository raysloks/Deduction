#include "LightSabotage.h"

#include "Game.h"

LightSabotage::LightSabotage()
{
}

LightSabotage::~LightSabotage()
{
}

void LightSabotage::call(Game & game, int64_t now)
{
	game.setLight(1.0f);
}
