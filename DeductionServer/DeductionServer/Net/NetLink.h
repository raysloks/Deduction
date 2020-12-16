#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <asio.hpp>

#include <map>

#include <mutex>
#include "AbilityUsed.h"
#include "DoorUpdate.h"
#include "GameOver.h"
#include "GamePhaseUpdate.h"
#include "GameSettings.h"
#include "GameStartRequested.h"
#include "GetAllPlayerPositions.h"
#include "GivenTasks.h"
#include "Heartbeat.h"
#include "HideAttempted.h"
#include "KillAttempted.h"
#include "LightUpdate.h"
#include "MeetingRequested.h"
#include "MobEjected.h"
#include "MobRemoved.h"
#include "MobRoleUpdate.h"
#include "MobStateUpdate.h"
#include "MobTeleport.h"
#include "MobUpdate.h"
#include "PhotoPose.h"
#include "PhotoTaken.h"
#include "PickupCooldown.h"
#include "PlayerUpdate.h"
#include "PlayerVoted.h"
#include "PresentEvidence.h"
#include "ReportAttempted.h"
#include "ResetGameSettings.h"
#include "RestartRequested.h"
#include "SabotageTaskUpdate.h"
#include "SendEvidence.h"
#include "SendSensorList.h"
#include "SmokeGrenadeActivate.h"
#include "TakePhoto.h"
#include "TaskListUpdate.h"
#include "TaskUpdate.h"
#include "TeleportToMeeting.h"
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
	void Send(const asio::ip::udp::endpoint& endpoint, const DoorUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameOver& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameSettings& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GetAllPlayerPositions& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const GivenTasks& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const HideAttempted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const LightUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobEjected& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PhotoPose& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PhotoTaken& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PickupCooldown& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const PresentEvidence& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const ResetGameSettings& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const SabotageTaskUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const SendEvidence& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const SendSensorList& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeActivate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TakePhoto& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const TeleportToMeeting& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message);
	static const uint32_t crc;
	asio::io_context io_context;
	asio::ip::udp::socket socket;
	std::map<asio::ip::udp::endpoint, int64_t> connections;
	std::mutex mutex;
};
