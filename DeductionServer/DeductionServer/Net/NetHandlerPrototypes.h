	void AcceptHandler(const asio::ip::udp::endpoint& endpoint);
	void ConnectHandler(const asio::ip::udp::endpoint& endpoint);
	void AbilityUsedHandler(const asio::ip::udp::endpoint& endpoint, const AbilityUsed& message);
	void GameSettingsHandler(const asio::ip::udp::endpoint& endpoint, const GameSettings& message);
	void GameStartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const GameStartRequested& message);
	void GetAllPlayerPositionsHandler(const asio::ip::udp::endpoint& endpoint, const GetAllPlayerPositions& message);
	void HeartbeatHandler(const asio::ip::udp::endpoint& endpoint, const Heartbeat& message);
	void HideAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const HideAttempted& message);
	void KillAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const KillAttempted& message);
	void MeetingRequestedHandler(const asio::ip::udp::endpoint& endpoint, const MeetingRequested& message);
	void MobUpdateHandler(const asio::ip::udp::endpoint& endpoint, const MobUpdate& message);
	void PhotoTakenHandler(const asio::ip::udp::endpoint& endpoint, const PhotoTaken& message);
	void PickupCooldownHandler(const asio::ip::udp::endpoint& endpoint, const PickupCooldown& message);
	void PlayerUpdateHandler(const asio::ip::udp::endpoint& endpoint, const PlayerUpdate& message);
	void PlayerVotedHandler(const asio::ip::udp::endpoint& endpoint, const PlayerVoted& message);
	void PresentEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const PresentEvidence& message);
	void PulseEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const PulseEvidence& message);
	void ReportAttemptedHandler(const asio::ip::udp::endpoint& endpoint, const ReportAttempted& message);
	void ResetGameSettingsHandler(const asio::ip::udp::endpoint& endpoint, const ResetGameSettings& message);
	void RestartRequestedHandler(const asio::ip::udp::endpoint& endpoint, const RestartRequested& message);
	void SabotageTaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const SabotageTaskUpdate& message);
	void SendEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const SendEvidence& message);
	void SendSensorListHandler(const asio::ip::udp::endpoint& endpoint, const SendSensorList& message);
	void SetMobColorHandler(const asio::ip::udp::endpoint& endpoint, const SetMobColor& message);
	void SmokeGrenadeActivateHandler(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeActivate& message);
	void SmokeGrenadeEvidenceHandler(const asio::ip::udp::endpoint& endpoint, const SmokeGrenadeEvidence& message);
	void TakePhotoHandler(const asio::ip::udp::endpoint& endpoint, const TakePhoto& message);
	void TaskUpdateHandler(const asio::ip::udp::endpoint& endpoint, const TaskUpdate& message);
	void TeleportToMeetingHandler(const asio::ip::udp::endpoint& endpoint, const TeleportToMeeting& message);
	void VoiceFrameHandler(const asio::ip::udp::endpoint& endpoint, const VoiceFrame& message);
