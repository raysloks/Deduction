#include "NetLink.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

// Application should implement this class using the prototypes in HandlerPrototypes.h
#include "../NetworkHandler.h"

const uint32_t NetLink::crc = 0x14b1ed65;
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
					break;
				}
				case 1:
				{
					handler->ConnectionHandler(endpoint);
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
	case 2:
	{
		GamePhaseUpdate message;
		message.deserialize(is);
		handler->GamePhaseUpdateHandler(endpoint, message);
		break;
	}
	case 3:
	{
		GameStartRequested message;
		message.deserialize(is);
		handler->GameStartRequestedHandler(endpoint, message);
		break;
	}
	case 4:
	{
		Heartbeat message;
		message.deserialize(is);
		handler->HeartbeatHandler(endpoint, message);
		break;
	}
	case 5:
	{
		KillAttempted message;
		message.deserialize(is);
		handler->KillAttemptedHandler(endpoint, message);
		break;
	}
	case 6:
	{
		MeetingRequested message;
		message.deserialize(is);
		handler->MeetingRequestedHandler(endpoint, message);
		break;
	}
	case 7:
	{
		MobRemoved message;
		message.deserialize(is);
		handler->MobRemovedHandler(endpoint, message);
		break;
	}
	case 8:
	{
		MobRoleUpdate message;
		message.deserialize(is);
		handler->MobRoleUpdateHandler(endpoint, message);
		break;
	}
	case 9:
	{
		MobStateUpdate message;
		message.deserialize(is);
		handler->MobStateUpdateHandler(endpoint, message);
		break;
	}
	case 10:
	{
		MobTeleport message;
		message.deserialize(is);
		handler->MobTeleportHandler(endpoint, message);
		break;
	}
	case 11:
	{
		MobUpdate message;
		message.deserialize(is);
		handler->MobUpdateHandler(endpoint, message);
		break;
	}
	case 12:
	{
		PlayerUpdate message;
		message.deserialize(is);
		handler->PlayerUpdateHandler(endpoint, message);
		break;
	}
	case 13:
	{
		PlayerVoted message;
		message.deserialize(is);
		handler->PlayerVotedHandler(endpoint, message);
		break;
	}
	case 14:
	{
		ReportAttempted message;
		message.deserialize(is);
		handler->ReportAttemptedHandler(endpoint, message);
		break;
	}
	case 15:
	{
		RestartRequested message;
		message.deserialize(is);
		handler->RestartRequestedHandler(endpoint, message);
		break;
	}
	case 16:
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

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(2);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(3);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(4);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(5);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(6);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(7);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(8);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(9);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(10);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(11);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(12);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(13);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(14);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(15);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void NetLink::Send(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(16);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

