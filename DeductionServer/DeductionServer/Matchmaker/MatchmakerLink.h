#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <asio.hpp>

#include <map>

#include "LobbyIdentity.h"
#include "LobbyRequest.h"

class MatchmakerHandler;

class MatchmakerLink
{
public:
	MatchmakerHandler * handler;
	MatchmakerLink();
	void Open(const asio::ip::udp::endpoint& endpoint);
	void Receive();
	void Connect(const asio::ip::udp::endpoint& endpoint);
	void Dispatch(asio::streambuf& buffer, const asio::ip::udp::endpoint& endpoint);
	void Send(const asio::ip::udp::endpoint& endpoint, const LobbyIdentity& message);
	void Send(const asio::ip::udp::endpoint& endpoint, const LobbyRequest& message);
	static const uint32_t crc;
	asio::io_context io_context;
	asio::ip::udp::socket socket;
	std::map<asio::ip::udp::endpoint, int64_t> connections;
};
