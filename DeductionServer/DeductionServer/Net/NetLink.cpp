#include "NetLink.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

// Application should implement this class using the prototypes in HandlerPrototypes.h
#include "../NetworkHandler.h"

const uint32_t NetLink::crc = 0x54047710;
NetLink::NetLink() : io_context(), socket(io_context)
{
}

void NetLink::Open(const asio::ip::udp::endpoint& endpoint)
{
	socket.open(endpoint.protocol());
	socket.bind(endpoint);
}

void NetLink::Receive()
{
	std::thread t([this]()
		{
			while (true)
			{
				try
				{
					asio::streambuf buffer(65507); 
					asio::ip::udp::endpoint endpoint;
					buffer.commit(socket.receive_from(buffer.prepare(65507), endpoint));
					Dispatch(buffer, endpoint);
				}
				catch (...)
				{
				}
			}
		}
	);
	t.detach();
}

void NetLink::Connect(const asio::ip::udp::endpoint& endpoint)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(0);
	os.write((const char*)&crc, sizeof(crc));
	os.put(0);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Dispatch(asio::streambuf& buffer, const asio::ip::udp::endpoint& endpoint)
{
	int64_t time = std::chrono::steady_clock::now().time_since_epoch().count();
	std::istream is(&buffer);
	char c = is.get();
	if (c == 0)
		{
			uint32_t remote_crc;
			is.read((char*)&remote_crc, sizeof(remote_crc));
			if (remote_crc != crc)
				return;
			connections[endpoint] = time;
			switch (is.get())
			{
				case 0:
				{
					std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
					std::ostream os(buffer.get());
					os.put(0);
					os.write((const char*)&crc, sizeof(crc));
					os.put(1);
					socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
					handler->AcceptHandler(endpoint);
					break;
				}
				case 1:
				{
					std::lock_guard lock(mutex);
					handler->ConnectHandler(endpoint);
					break;
				}
				default:
					break;
			}
			return;
		}
	auto it = connections.find(endpoint);
	if (it == connections.end())
		return;
	if (time - it->second > 10'000'000'000)
	{
		connections.erase(it);
		return;
	}
	connections[endpoint] = time;
	switch (c)
	{
	case 1:
	{
		AbilityUsed message;
		message.deserialize(is);
		handler->AbilityUsedHandler(endpoint, message);
		break;
	}
	case 6:
	{
		GameSettings message;
		message.deserialize(is);
		handler->GameSettingsHandler(endpoint, message);
		break;
	}
	case 7:
	{
		GameStartRequested message;
		message.deserialize(is);
		handler->GameStartRequestedHandler(endpoint, message);
		break;
	}
	case 8:
	{
		GetAllPlayerPositions message;
		message.deserialize(is);
		handler->GetAllPlayerPositionsHandler(endpoint, message);
		break;
	}
	case 10:
	{
		Heartbeat message;
		message.deserialize(is);
		handler->HeartbeatHandler(endpoint, message);
		break;
	}
	case 11:
	{
		HideAttempted message;
		message.deserialize(is);
		handler->HideAttemptedHandler(endpoint, message);
		break;
	}
	case 12:
	{
		KillAttempted message;
		message.deserialize(is);
		handler->KillAttemptedHandler(endpoint, message);
		break;
	}
	case 14:
	{
		MeetingRequested message;
		message.deserialize(is);
		handler->MeetingRequestedHandler(endpoint, message);
		break;
	}
	case 20:
	{
		MobUpdate message;
		message.deserialize(is);
		handler->MobUpdateHandler(endpoint, message);
		break;
	}
	case 21:
	{
		PhotoTaken message;
		message.deserialize(is);
		handler->PhotoTakenHandler(endpoint, message);
		break;
	}
	case 22:
	{
		PickupCooldown message;
		message.deserialize(is);
		handler->PickupCooldownHandler(endpoint, message);
		break;
	}
	case 23:
	{
		PlayerUpdate message;
		message.deserialize(is);
		handler->PlayerUpdateHandler(endpoint, message);
		break;
	}
	case 24:
	{
		PlayerVoted message;
		message.deserialize(is);
		handler->PlayerVotedHandler(endpoint, message);
		break;
	}
	case 25:
	{
		PresentEvidence message;
		message.deserialize(is);
		handler->PresentEvidenceHandler(endpoint, message);
		break;
	}
	case 26:
	{
		PulseEvidence message;
		message.deserialize(is);
		handler->PulseEvidenceHandler(endpoint, message);
		break;
	}
	case 27:
	{
		ReportAttempted message;
		message.deserialize(is);
		handler->ReportAttemptedHandler(endpoint, message);
		break;
	}
	case 28:
	{
		ResetGameSettings message;
		message.deserialize(is);
		handler->ResetGameSettingsHandler(endpoint, message);
		break;
	}
	case 29:
	{
		RestartRequested message;
		message.deserialize(is);
		handler->RestartRequestedHandler(endpoint, message);
		break;
	}
	case 30:
	{
		SabotageTaskUpdate message;
		message.deserialize(is);
		handler->SabotageTaskUpdateHandler(endpoint, message);
		break;
	}
	case 31:
	{
		SendEvidence message;
		message.deserialize(is);
		handler->SendEvidenceHandler(endpoint, message);
		break;
	}
	case 32:
	{
		SendSensorList message;
		message.deserialize(is);
		handler->SendSensorListHandler(endpoint, message);
		break;
	}
	case 33:
	{
		SetMobColor message;
		message.deserialize(is);
		handler->SetMobColorHandler(endpoint, message);
		break;
	}
	case 34:
	{
		SmokeGrenadeActivate message;
		message.deserialize(is);
		handler->SmokeGrenadeActivateHandler(endpoint, message);
		break;
	}
	case 35:
	{
		SmokeGrenadeEvidence message;
		message.deserialize(is);
		handler->SmokeGrenadeEvidenceHandler(endpoint, message);
		break;
	}
	case 36:
	{
		TakePhoto message;
		message.deserialize(is);
		handler->TakePhotoHandler(endpoint, message);
		break;
	}
	case 38:
	{
		TaskUpdate message;
		message.deserialize(is);
		handler->TaskUpdateHandler(endpoint, message);
		break;
	}
	case 39:
	{
		TeleportToMeeting message;
		message.deserialize(is);
		handler->TeleportToMeetingHandler(endpoint, message);
		break;
	}
	case 40:
	{
		VoiceFrame message;
		message.deserialize(is);
		handler->VoiceFrameHandler(endpoint, message);
		break;
	}
	default:
		break;
	}
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const AbilityUsed& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(1);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const ColorTaken& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(2);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const DoorUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(3);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GameOver& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(4);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(5);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GameSettings& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(6);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GetAllPlayerPositions& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(8);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GivenTasks& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(9);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(10);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const HideAttempted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(11);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(12);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const LightUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(13);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(14);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobEjected& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(15);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(16);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(17);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(18);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(19);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(20);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PhotoTaken& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(21);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PickupCooldown& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(22);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(23);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(24);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PresentEvidence& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(25);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PulseEvidence& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(26);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(27);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const SabotageTaskUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(30);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const SendEvidence& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(31);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const SendSensorList& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(32);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeActivate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(34);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeEvidence& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(35);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(37);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(38);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const TeleportToMeeting& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(39);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(40);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

