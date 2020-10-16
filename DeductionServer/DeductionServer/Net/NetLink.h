#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <asio.hpp>

#include <map>

#include "AbilityUsed.h"
#include "GamePhaseUpdate.h"
#include "GameSettingSet.h"
#include "GameSettingsUpdate.h"
#include "GameStartRequested.h"
#include "Heartbeat.h"
#include "KillAttempted.h"
#include "MeetingRequested.h"
#include "MobRemoved.h"
#include "MobRoleUpdate.h"
#include "MobStateUpdate.h"
#include "MobTeleport.h"
#include "MobUpdate.h"
#include "PlayerUpdate.h"
#include "PlayerVoted.h"
#include "ReportAttempted.h"
#include "RestartRequested.h"
#include "TaskListUpdate.h"
#include "TaskUpdate.h"
#include "VoiceFrame.h"

class NetworkHandler;

class NetLink
{
public:
	NetworkHandler * handler;
	NetLink();
	void Open(const asio::ip::udp::endpoint& endpoint);
	void Receive();
	void Connect(const asio::ip::udp::endpoint& endpoint);
	void Dispatch(asio::streambuf& buffer, const asio::ip::udp::endpoint& endpoint);
	void Send(const asio::ip::udp::endpoint& endpoint, const AbilityUsed& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameSettingSet& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameSettingsUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message);
	static const uint32_t crc;
	asio::io_context io_context;
	asio::ip::udp::socket socket;
	std::map<asio::ip::udp::endpoint, int64_t> connections;
};
