#pragma once

#include <asio.hpp>

#include <vector>
#include <map>

#include "Matchmaker/MatchmakerLink.h"

#include "Xoroshiro128Plus.h"

class MatchmakerHandler
{
public:
	MatchmakerHandler();

	std::string getRandomLobby();
	std::string getFreeLobby();

	void addServer(const std::string& lobby, const asio::ip::udp::endpoint& server);

	void ConnectionHandler(const asio::ip::udp::endpoint& endpoint);
	void LobbyIdentityHandler(const asio::ip::udp::endpoint& endpoint, const LobbyIdentity& message);
	void LobbyRequestHandler(const asio::ip::udp::endpoint& endpoint, const LobbyRequest& message);

	MatchmakerLink link;

	Xoroshiro128Plus rng;

	std::vector<std::string> vacant_lobbies;
	std::map<std::string, asio::ip::udp::endpoint> servers;
	std::map<asio::ip::udp::endpoint, std::string> lobbies;
};

