using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct ColorTaken
{
	public int id;
	public int former;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((int)id);
		writer.Write((int)former);
	}

	public static ColorTaken Deserialize(BinaryReader reader)
	{
		ColorTaken _ret = new ColorTaken();
		_ret.id = (int)reader.ReadInt32();
		_ret.former = (int)reader.ReadInt32();
		return _ret;
	}
};
