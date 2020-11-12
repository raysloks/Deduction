using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SendEvidence
{
	public List<byte> picture;
	public ulong id;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.picture.Count;
			writer.Write(size);
			foreach (var i in this.picture)
		writer.Write((byte)i);
		}
		writer.Write((ulong)id);
	}

	public static SendEvidence Deserialize(BinaryReader reader)
	{
		SendEvidence _ret = new SendEvidence();
	{
		_ret.picture = new List<byte>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			byte element;
		element = (byte)reader.ReadByte();
			_ret.picture.Add(element);
		}
	}
		_ret.id = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
