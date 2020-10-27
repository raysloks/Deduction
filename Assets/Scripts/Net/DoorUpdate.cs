using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct DoorUpdate
{
	public ushort door;
	public bool open;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ushort)door);
		writer.Write((bool)open);
	}

	public static DoorUpdate Deserialize(BinaryReader reader)
	{
		DoorUpdate _ret = new DoorUpdate();
		_ret.door = (ushort)reader.ReadUInt16();
		_ret.open = (bool)reader.ReadBoolean();
		return _ret;
	}
};
