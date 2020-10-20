#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <iostream>

#pragma pack(push, 1)
class GameSettings
{
public:
	int64_t impostorCount;
	int64_t votesPerPlayer;
	int64_t emergencyMeetingsPerPlayer;
	int64_t emergencyMeetingCooldown;
	int64_t killCooldown;
	int64_t voteTime;
	int64_t discussionTime;
	bool killVictoryEnabled;
	float crewmateVision;
	float impostorVision;
	float playerSpeed;
	bool killOnTies;
	bool enableSkipButton;
	bool showVotesWhenEveryoneHasVoted;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
