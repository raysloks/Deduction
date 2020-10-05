using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct Heartbeat
{
	public long time;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
	}

	public static Heartbeat Deserialize(BinaryReader reader)
	{
		Heartbeat _ret = new Heartbeat();
		_ret.time = (long)reader.ReadInt64();
		return _ret;
	}
};
