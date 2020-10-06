using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct AbilityUsed
{
	public long time;
	public ulong ability;
	public ulong target;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
		writer.Write((ulong)ability);
		writer.Write((ulong)target);
	}

	public static AbilityUsed Deserialize(BinaryReader reader)
	{
		AbilityUsed _ret = new AbilityUsed();
		_ret.time = (long)reader.ReadInt64();
		_ret.ability = (ulong)reader.ReadUInt64();
		_ret.target = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
