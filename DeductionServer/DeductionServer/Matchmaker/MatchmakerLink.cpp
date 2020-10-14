#include "MatchmakerLink.h"

// WARNING : Auto-generated file, changes made will disappear when re-generated.

// Application should implement this class using the prototypes in HandlerPrototypes.h
#include "../MatchmakerHandler.h"

const uint32_t MatchmakerLink::crc = 0xacd77b6f;
MatchmakerLink::MatchmakerLink() : io_context(), socket(io_context)
{
}

void MatchmakerLink::Open(const asio::ip::udp::endpoint& endpoint)
{
	socket.open(endpoint.protocol());
	socket.bind(endpoint);
}

void MatchmakerLink::Receive()
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

void MatchmakerLink::Connect(const asio::ip::udp::endpoint& endpoint)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(0);
	os.write((const char*)&crc, sizeof(crc));
	os.put(0);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void MatchmakerLink::Dispatch(asio::streambuf& buffer, const asio::ip::udp::endpoint& endpoint)
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
		LobbyIdentity message;
		message.deserialize(is);
		handler->LobbyIdentityHandler(endpoint, message);
		break;
	}
	case 2:
	{
		LobbyRequest message;
		message.deserialize(is);
		handler->LobbyRequestHandler(endpoint, message);
		break;
	}
	default:
		break;
	}
}

void MatchmakerLink::Send(const asio::ip::udp::endpoint& endpoint, const LobbyIdentity& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(1);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

void MatchmakerLink::Send(const asio::ip::udp::endpoint& endpoint, const LobbyRequest& message)
{
	std::shared_ptr<asio::streambuf> buffer = std::make_shared<asio::streambuf>();
	std::ostream os(buffer.get());
	os.put(2);
	message.serialize(os);
	socket.async_send_to(buffer->data(), endpoint, [buffer](const asio::error_code&, size_t) {});
}

