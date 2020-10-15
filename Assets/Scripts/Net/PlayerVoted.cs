using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PlayerVoted
{
	public ulong phase;
	public long timer;
	public ulong id;
	public long totalVotes;
	public string buttonName;
	public long votesLeft;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)phase);
		writer.Write((long)timer);
		writer.Write((ulong)id);
		writer.Write((long)totalVotes);
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(buttonName);
			ushort size = (ushort)bytes.Length;
			writer.Write(size);
			writer.Write(bytes);
		}
		writer.Write((long)votesLeft);
	}

	public static PlayerVoted Deserialize(BinaryReader reader)
	{
		PlayerVoted _ret = new PlayerVoted();
		_ret.phase = (ulong)reader.ReadUInt64();
		_ret.timer = (long)reader.ReadInt64();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.totalVotes = (long)reader.ReadInt64();
		{
			ushort size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(size);
			_ret.buttonName = System.Text.Encoding.UTF8.GetString(bytes);
		}
		_ret.votesLeft = (long)reader.ReadInt64();
		return _ret;
	}
};
