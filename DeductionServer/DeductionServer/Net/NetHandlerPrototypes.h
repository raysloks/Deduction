	void ConnectionHandler(const asio::ip::udp::endpoint& endpoint);
	void AbilityUsedHandler(const asio::ip::udp::endpoint& endpoint, const AbilityUsed& message);
	void GameOverHandler(const asio::ip::udp::endpoint& endpoint, const GameOver& message);
	void GamePhaseUpdateHandler(const asio::ip::udp::endpoint& endpoint, const GamePhaseUpdate& message);
	void GameSettingsHandler(const asio::ip::udp::endpoint& endpoint, const GameSettings& message);
	void GameStartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message);
	void GivenTasksHandler(const asio::ip::udp::endpoint& endpoint, const GivenTasks& message);
	void HeartbeatHandler(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message);
	void KillAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message);
	void MeetingRequestedHandler(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message);
	void MobRemovedHandler(const asio::ip::udp::endpoint& endpoint, const MobRemoved& message);
	void MobRoleUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobRoleUpdate& message);
	void MobStateUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobStateUpdate& message);
	void MobTeleportHandler(const asio::ip::udp::endpoint& endpoint, const MobTeleport& message);
	void MobUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message);
	void PlayerUpdateHandler(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message);
	void PlayerVotedHandler(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message);
	void ReportAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message);
	void ResetGameSettingsHandler(const asio::ip::udp::endpoint& endpoint, const ResetGameSettings& message);
	void RestartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message);
	void TaskListUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskListUpdate& message);
	void TaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message);
	void VoiceFrameHandler(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message);
