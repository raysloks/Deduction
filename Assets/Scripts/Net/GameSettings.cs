using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameSettings
{
	public int map;
	public long impostorCount;
	public long votesPerPlayer;
	public long emergencyMeetingsPerPlayer;
	public long emergencyMeetingCooldown;
	public long killCooldown;
	public float killRange;
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
	public long shortTaskCount;
	public long longTaskCount;
	public long taskbarUpdateStyle;
	public long sabotageCooldown;
	public bool gameOverEnabled;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((int)map);
		writer.Write((long)impostorCount);
		writer.Write((long)votesPerPlayer);
		writer.Write((long)emergencyMeetingsPerPlayer);
		writer.Write((long)emergencyMeetingCooldown);
		writer.Write((long)killCooldown);
		writer.Write((float)killRange);
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
		writer.Write((long)shortTaskCount);
		writer.Write((long)longTaskCount);
		writer.Write((long)taskbarUpdateStyle);
		writer.Write((long)sabotageCooldown);
		writer.Write((bool)gameOverEnabled);
	}

	public static GameSettings Deserialize(BinaryReader reader)
	{
		GameSettings _ret = new GameSettings();
		_ret.map = (int)reader.ReadInt32();
		_ret.impostorCount = (long)reader.ReadInt64();
		_ret.votesPerPlayer = (long)reader.ReadInt64();
		_ret.emergencyMeetingsPerPlayer = (long)reader.ReadInt64();
		_ret.emergencyMeetingCooldown = (long)reader.ReadInt64();
		_ret.killCooldown = (long)reader.ReadInt64();
		_ret.killRange = (float)reader.ReadSingle();
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
		_ret.shortTaskCount = (long)reader.ReadInt64();
		_ret.longTaskCount = (long)reader.ReadInt64();
		_ret.taskbarUpdateStyle = (long)reader.ReadInt64();
		_ret.sabotageCooldown = (long)reader.ReadInt64();
		_ret.gameOverEnabled = (bool)reader.ReadBoolean();
		return _ret;
	}
};
