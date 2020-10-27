using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PlayerVoted
{
	public ulong voter;
	public ulong target;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)voter);
		writer.Write((ulong)target);
	}

	public static PlayerVoted Deserialize(BinaryReader reader)
	{
		PlayerVoted _ret = new PlayerVoted();
		_ret.voter = (ulong)reader.ReadUInt64();
		_ret.target = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
