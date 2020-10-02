using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct Heartbeat
{
	public ulong time;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)time);
	}

	public static Heartbeat Deserialize(BinaryReader reader)
	{
		Heartbeat _ret = new Heartbeat();
		_ret.time = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
