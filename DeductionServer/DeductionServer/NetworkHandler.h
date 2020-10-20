#pragma once

#include <chrono>
#include <asio.hpp>

#include "Net/NetLink.h"

#include "Game.h"

#include "Player.h"
#include "Mob.h"

#include "Xoroshiro128Plus.h"

class NetworkHandler
{
public:
	NetworkHandler();

	void tick(const std::chrono::steady_clock::time_point& now);

	uint64_t createMob(const Mob& mob = Mob());
	void createPlayer(const asio::ip::udp::endpoint& endpoint, const std::string& name);

	void updateMobState(uint64_t id);
	void updateMobStatesForPlayer(const asio::ip::udp::endpoint& endpoint);
	void updateMobStates();

	void updateMobRoles();

	void removeMob(uint64_t id);

	void killMob(uint64_t id);

	void ConnectionHandler(const asio::ip::udp::endpoint& endpoint);
	void AbilityUsedHandler(const asio::ip::udp::endpoint& endpoint, const AbilityUsed& message);
	void GameOverHandler(const asio::ip::udp::endpoint& endpoint, const GameOver& message);
	void GamePhaseUpdateHandler(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message);
	void GameSettingSetHandler(const asio::ip::udp::endpoint& endpoint, const GameSettingSet& message);
	void GameSettingsUpdateHandler(const asio::ip::udp::endpoint& endpoint, const GameSettingsUpdate& message);
	void GameStartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message);
	void GivenTasksHandler(const asio::ip::udp::endpoint& endpoint, const GivenTasks& message);
	void HeartbeatHandler(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message);
	void KillAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message);
	void MeetingRequestedHandler(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message);
	void MobRemovedHandler(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message);
	void MobRoleUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message);
	void MobStateUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message);
	void MobTeleportHandler(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message);
	void MobUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message);
	void PlayerUpdateHandler(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message);
	void PlayerVotedHandler(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message);
	void ReportAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message);
	void RestartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message);
	void TaskListUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message);
	void TaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message);
	void VoiceFrameHandler(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message);

	template <class message_type>
	void Broadcast(const message_type& message)
	{
		for (auto player : players)
			link.Send(player.first, message);
	}

	NetLink link;

	Xoroshiro128Plus rng;

	int64_t time;

	std::map<asio::ip::udp::endpoint, Player> players;
	std::vector<Mob> mobs;

	Game game;
};

