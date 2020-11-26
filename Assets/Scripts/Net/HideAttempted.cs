using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct HideAttempted
{
	public int index;
	public ulong user;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((int)index);
		writer.Write((ulong)user);
	}

	public static HideAttempted Deserialize(BinaryReader reader)
	{
		HideAttempted _ret = new HideAttempted();
		_ret.index = (int)reader.ReadInt32();
		_ret.user = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
