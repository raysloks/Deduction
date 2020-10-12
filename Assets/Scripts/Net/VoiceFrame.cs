using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct VoiceFrame
{
	public ulong id;
	public List<byte> data;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		{
			ushort size = (ushort)this.data.Count;
			writer.Write(size);
			foreach (var i in this.data)
		writer.Write((byte)i);
		}
	}

	public static VoiceFrame Deserialize(BinaryReader reader)
	{
		VoiceFrame _ret = new VoiceFrame();
		_ret.id = (ulong)reader.ReadUInt64();
	{
		_ret.data = new List<byte>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			byte element;
		element = (byte)reader.ReadByte();
			_ret.data.Add(element);
		}
	}
		return _ret;
	}
};
