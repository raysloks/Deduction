using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobStateUpdate
{
	public MobUpdate update;
	public ulong type;
	public ulong state;
	public ulong sprite;

	public void Serialize(BinaryWriter writer)
	{
		update.Serialize(writer);
		writer.Write((ulong)type);
		writer.Write((ulong)state);
		writer.Write((ulong)sprite);
	}

	public static MobStateUpdate Deserialize(BinaryReader reader)
	{
		MobStateUpdate _ret = new MobStateUpdate();
		_ret.update = MobUpdate.Deserialize(reader);
		_ret.type = (ulong)reader.ReadUInt64();
		_ret.state = (ulong)reader.ReadUInt64();
		_ret.sprite = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
