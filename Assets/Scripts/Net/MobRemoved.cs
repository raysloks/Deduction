using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobRemoved
{
	public ulong id;
	public long time;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((long)time);
	}

	public static MobRemoved Deserialize(BinaryReader reader)
	{
		MobRemoved _ret = new MobRemoved();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.time = (long)reader.ReadInt64();
		return _ret;
	}
};