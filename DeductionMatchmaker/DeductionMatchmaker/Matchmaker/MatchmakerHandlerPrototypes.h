	void ConnectionHandler(const asio::ip::udp::endpoint& endpoint);
	void LobbyIdentityHandler(const asio::ip::udp::endpoint& endpoint, const LobbyIdentity& message);
	void LobbyRequestHandler(const asio::ip::udp::endpoint& endpoint, const LobbyRequest& message);
