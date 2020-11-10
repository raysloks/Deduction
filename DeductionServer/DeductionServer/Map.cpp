#include "Map.h"

#include "Coal.h"

#include "LightSabotage.h"
#include "DoorSabotage.h"
#include "VoiceSabotage.h"
#include "NukeSabotage.h"

Map::Map()
{
}

Map::Map(const Coal& coal)
{
	spawnPos = coal["spawnPos"];
	spawnSize = coal["spawnSize"];
	meetingPos = coal["meetingPos"];
	meetingSize = coal["meetingSize"];

	for (auto element : coal["sabotages"].elements)
	{
		std::string type = element["type"].string;
		if (type == "LightSabotage")
		{
			auto lights = new LightSabotage();
			lights->duration = element["duration"].real * 1'000'000'000;
			lights->minigame_index = element["minigame_index"].integer;
			sabotages.push_back(std::unique_ptr<Sabotage>(lights));
		}
		if (type == "DoorSabotage")
		{
			auto doors = new DoorSabotage();
			for (auto door : element["doors"].elements)
				doors->doors.push_back(door.integer);
			doors->duration = element["duration"].real * 1'000'000'000;
			doors->minigame_index = element["minigame_index"].integer;
			sabotages.push_back(std::unique_ptr<Sabotage>(doors));
		}
		if (type == "VoiceSabotage")
		{
			auto voice = new VoiceSabotage();
			voice->duration = element["duration"].real * 1'000'000'000;
			voice->minigame_index = element["minigame_index"].integer;
			sabotages.push_back(std::unique_ptr<Sabotage>(voice));
		}
		if (type == "NukeSabotage")
		{
			auto nuke = new NukeSabotage();
			nuke->duration = element["duration"].real * 1'000'000'000;
			nuke->minigame_index = element["minigame_index"].integer;
			sabotages.push_back(std::unique_ptr<Sabotage>(nuke));
		}
	}
}
