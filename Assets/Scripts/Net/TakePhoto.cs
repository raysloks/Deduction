using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct TakePhoto
{
	public uint index;
	public ulong photographer;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((uint)index);
		writer.Write((ulong)photographer);
	}

	public static TakePhoto Deserialize(BinaryReader reader)
	{
		TakePhoto _ret = new TakePhoto();
		_ret.index = (uint)reader.ReadUInt32();
		_ret.photographer = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
