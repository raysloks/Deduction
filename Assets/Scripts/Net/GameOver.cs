using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameOver
{
	public List<ulong> winners;
	public ulong role;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.winners.Count;
			writer.Write(size);
			foreach (var i in this.winners)
		writer.Write((ulong)i);
		}
		writer.Write((ulong)role);
	}

	public static GameOver Deserialize(BinaryReader reader)
	{
		GameOver _ret = new GameOver();
	{
		_ret.winners = new List<ulong>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			ulong element;
		element = (ulong)reader.ReadUInt64();
			_ret.winners.Add(element);
		}
	}
		_ret.role = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
