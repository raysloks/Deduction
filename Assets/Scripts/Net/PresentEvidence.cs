using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PresentEvidence
{
	public ulong index;
	public ulong presenter;
	public List<byte> sensornames;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)index);
		writer.Write((ulong)presenter);
		{
			ushort size = (ushort)this.sensornames.Count;
			writer.Write(size);
			foreach (var i in this.sensornames)
		writer.Write((byte)i);
		}
	}

	public static PresentEvidence Deserialize(BinaryReader reader)
	{
		PresentEvidence _ret = new PresentEvidence();
		_ret.index = (ulong)reader.ReadUInt64();
		_ret.presenter = (ulong)reader.ReadUInt64();
	{
		_ret.sensornames = new List<byte>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			byte element;
		element = (byte)reader.ReadByte();
			_ret.sensornames.Add(element);
		}
	}
		return _ret;
	}
};
