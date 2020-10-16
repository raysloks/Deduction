#include "MatchmakerHandler.h"

#include "NetworkHandler.h"

MatchmakerHandler::MatchmakerHandler()
{
	link.handler = this;
	link.Open(asio::ip::udp::endpoint(asio::ip::udp::v4(), 0));
	link.Receive();
}

void MatchmakerHandler::UpdateLobby()
{
	link.Connect(asio::ip::udp::endpoint(asio::ip::address::from_string("172.105.79.194"), 58712));
}

void MatchmakerHandler::ConnectionHandler(const asio::ip::udp::endpoint & endpoint)
{
	LobbyIdentity message;
	message.port = handler->link.socket.local_endpoint().port();
	link.Send(endpoint, message);
}

void MatchmakerHandler::LobbyIdentityHandler(const asio::ip::udp::endpoint & endpoint, const LobbyIdentity & message)
{
	std::cout << message.lobby << std::endl;
}

void MatchmakerHandler::LobbyRequestHandler(const asio::ip::udp::endpoint & endpoint, const LobbyRequest & message)
{
}