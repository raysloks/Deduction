using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct KillAttempted
{
	public long time;
	public ulong target;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
		writer.Write((ulong)target);
	}

	public static KillAttempted Deserialize(BinaryReader reader)
	{
		KillAttempted _ret = new KillAttempted();
		_ret.time = (long)reader.ReadInt64();
		_ret.target = (ulong)reader.ReadUInt64();
		return _ret;
	}
};