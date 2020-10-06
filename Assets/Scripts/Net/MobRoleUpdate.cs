using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobRoleUpdate
{
	public ulong id;
	public ulong role;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((ulong)role);
	}

	public static MobRoleUpdate Deserialize(BinaryReader reader)
	{
		MobRoleUpdate _ret = new MobRoleUpdate();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.role = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
