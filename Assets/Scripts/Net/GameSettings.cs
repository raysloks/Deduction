using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameSettings
{
	public long impostorCount;
	public long votesPerPlayer;
	public long emergencyMeetingsPerPlayer;
	public long emergencyMeetingCooldown;
	public long killCooldown;
	public long voteTime;
	public long discussionTime;
	public bool killVictoryEnabled;
	public float crewmateVision;
	public float impostorVision;
	public float playerSpeed;
	public bool killOnTies;
	public bool enableSkipButton;
	public bool showVotesWhenEveryoneHasVoted;
	public bool anonymousVotes;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)impostorCount);
		writer.Write((long)votesPerPlayer);
		writer.Write((long)emergencyMeetingsPerPlayer);
		writer.Write((long)emergencyMeetingCooldown);
		writer.Write((long)killCooldown);
		writer.Write((long)voteTime);
		writer.Write((long)discussionTime);
		writer.Write((bool)killVictoryEnabled);
		writer.Write((float)crewmateVision);
		writer.Write((float)impostorVision);
		writer.Write((float)playerSpeed);
		writer.Write((bool)killOnTies);
		writer.Write((bool)enableSkipButton);
		writer.Write((bool)showVotesWhenEveryoneHasVoted);
		writer.Write((bool)anonymousVotes);
	}

	public static GameSettings Deserialize(BinaryReader reader)
	{
		GameSettings _ret = new GameSettings();
		_ret.impostorCount = (long)reader.ReadInt64();
		_ret.votesPerPlayer = (long)reader.ReadInt64();
		_ret.emergencyMeetingsPerPlayer = (long)reader.ReadInt64();
		_ret.emergencyMeetingCooldown = (long)reader.ReadInt64();
		_ret.killCooldown = (long)reader.ReadInt64();
		_ret.voteTime = (long)reader.ReadInt64();
		_ret.discussionTime = (long)reader.ReadInt64();
		_ret.killVictoryEnabled = (bool)reader.ReadBoolean();
		_ret.crewmateVision = (float)reader.ReadSingle();
		_ret.impostorVision = (float)reader.ReadSingle();
		_ret.playerSpeed = (float)reader.ReadSingle();
		_ret.killOnTies = (bool)reader.ReadBoolean();
		_ret.enableSkipButton = (bool)reader.ReadBoolean();
		_ret.showVotesWhenEveryoneHasVoted = (bool)reader.ReadBoolean();
		_ret.anonymousVotes = (bool)reader.ReadBoolean();
		return _ret;
	}
};
