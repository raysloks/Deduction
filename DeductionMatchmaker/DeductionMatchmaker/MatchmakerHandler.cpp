#include "MatchmakerHandler.h"

MatchmakerHandler::MatchmakerHandler()
{
	link.handler = this;
	link.Open(asio::ip::udp::endpoint(asio::ip::udp::v4(), 58712));
	link.Receive();
}

std::string MatchmakerHandler::getRandomLobby()
{
	std::string lobby;
	for (size_t i = 0; i < 4; ++i)
		lobby += (char)rng.next('A', 'Z');
	return lobby;
}

std::string MatchmakerHandler::getFreeLobby()
{
	std::string lobby;
	do
	{
		lobby = getRandomLobby();
	} while (servers.find(lobby) != servers.end());
	return lobby;
}

void MatchmakerHandler::addServer(const std::string & lobby, const asio::ip::udp::endpoint & server)
{
	auto it = lobbies.find(server);
	if (it != lobbies.end())
		servers.erase(it->second);

	lobbies[server] = lobby;
	servers[lobby] = server;
}

void MatchmakerHandler::ConnectionHandler(const asio::ip::udp::endpoint & endpoint)
{
}

void MatchmakerHandler::LobbyIdentityHandler(const asio::ip::udp::endpoint & endpoint, const LobbyIdentity & message)
{
	std::string lobby = getFreeLobby();
	addServer(lobby, asio::ip::udp::endpoint(endpoint.address(), message.port));
	LobbyIdentity reply;
	reply.lobby = lobby;
	link.Send(endpoint, reply);
}

void MatchmakerHandler::LobbyRequestHandler(const asio::ip::udp::endpoint & endpoint, const LobbyRequest & message)
{
	if (message.lobby.empty())
	{
		while (vacant_lobbies.size() > 0)
		{
			auto lobby = vacant_lobbies.front();
			vacant_lobbies.pop();
			auto it = servers.find(lobby);
			if (it != servers.end())
			{
				LobbyIdentity reply;
				reply.lobby = lobby;
				reply.address = it->second.address().to_string();
				reply.port = it->second.port();
				link.Send(endpoint, reply);
				return;
			}
		}
	}

	auto it = servers.find(message.lobby);
	if (it != servers.end())
	{
		LobbyIdentity reply;
		reply.lobby = message.lobby;
		reply.address = it->second.address().to_string();
		reply.port = it->second.port();
		link.Send(endpoint, reply);
	}
	else
	{
		LobbyIdentity reply;
		reply.lobby = message.lobby;
		reply.address = "";
		reply.port = 0;
		link.Send(endpoint, reply);
	}
}
