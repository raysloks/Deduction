#pragma once

#include <asio.hpp>
#include <string>

#include "Matchmaker/MatchmakerLink.h"

class NetworkHandler;

class MatchmakerHandler
{
public:
	MatchmakerHandler();

	void UpdateLobby();

	void ConnectionHandler(const asio::ip::udp::endpoint& endpoint);
	void LobbyIdentityHandler(const asio::ip::udp::endpoint& endpoint, const LobbyIdentity& message);
	void LobbyRequestHandler(const asio::ip::udp::endpoint& endpoint, const LobbyRequest& message);

	MatchmakerLink link;

	NetworkHandler * handler;

	std::string lobby;
};

