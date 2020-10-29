using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobEjected
{
	public ulong id;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
	}

	public static MobEjected Deserialize(BinaryReader reader)
	{
		MobEjected _ret = new MobEjected();
		_ret.id = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
