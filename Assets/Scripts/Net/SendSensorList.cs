using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SendSensorList
{
	public List<byte> names;
	public List<ulong> times;
	public ulong player;
	public List<ulong> playerIds;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.names.Count;
			writer.Write(size);
			foreach (var i in this.names)
		writer.Write((byte)i);
		}
		{
			ushort size = (ushort)this.times.Count;
			writer.Write(size);
			foreach (var i in this.times)
		writer.Write((ulong)i);
		}
		writer.Write((ulong)player);
		{
			ushort size = (ushort)this.playerIds.Count;
			writer.Write(size);
			foreach (var i in this.playerIds)
		writer.Write((ulong)i);
		}
	}

	public static SendSensorList Deserialize(BinaryReader reader)
	{
		SendSensorList _ret = new SendSensorList();
	{
		_ret.names = new List<byte>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			byte element;
		element = (byte)reader.ReadByte();
			_ret.names.Add(element);
		}
	}
	{
		_ret.times = new List<ulong>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			ulong element;
		element = (ulong)reader.ReadUInt64();
			_ret.times.Add(element);
		}
	}
		_ret.player = (ulong)reader.ReadUInt64();
	{
		_ret.playerIds = new List<ulong>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			ulong element;
		element = (ulong)reader.ReadUInt64();
			_ret.playerIds.Add(element);
		}
	}
		return _ret;
	}
};
