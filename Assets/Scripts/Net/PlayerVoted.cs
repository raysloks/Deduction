using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PlayerVoted
{
	public ulong phase;
	public long timer;
	public ulong id;
	public long totalVotes;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)phase);
		writer.Write((long)timer);
		writer.Write((ulong)id);
		writer.Write((long)totalVotes);
	}

	public static PlayerVoted Deserialize(BinaryReader reader)
	{
		PlayerVoted _ret = new PlayerVoted();
		_ret.phase = (ulong)reader.ReadUInt64();
		_ret.timer = (long)reader.ReadInt64();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.totalVotes = (long)reader.ReadInt64();
		return _ret;
	}
};
