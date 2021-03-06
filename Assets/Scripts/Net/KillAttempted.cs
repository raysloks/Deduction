using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct KillAttempted
{
	public long time;
	public ulong target;
	public ulong killer;
	public bool knife;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
		writer.Write((ulong)target);
		writer.Write((ulong)killer);
		writer.Write((bool)knife);
	}

	public static KillAttempted Deserialize(BinaryReader reader)
	{
		KillAttempted _ret = new KillAttempted();
		_ret.time = (long)reader.ReadInt64();
		_ret.target = (ulong)reader.ReadUInt64();
		_ret.killer = (ulong)reader.ReadUInt64();
		_ret.knife = (bool)reader.ReadBoolean();
		return _ret;
	}
};
