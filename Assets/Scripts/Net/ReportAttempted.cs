using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct ReportAttempted
{
	public long time;
	public ulong idOfInitiator;
	public ulong target;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
		writer.Write((ulong)idOfInitiator);
		writer.Write((ulong)target);
	}

	public static ReportAttempted Deserialize(BinaryReader reader)
	{
		ReportAttempted _ret = new ReportAttempted();
		_ret.time = (long)reader.ReadInt64();
		_ret.idOfInitiator = (ulong)reader.ReadUInt64();
		_ret.target = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
