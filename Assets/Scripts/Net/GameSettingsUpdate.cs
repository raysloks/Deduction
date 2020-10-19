using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameSettingsUpdate
{
	public List<long> values;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.values.Count;
			writer.Write(size);
			foreach (var i in this.values)
		writer.Write((long)i);
		}
	}

	public static GameSettingsUpdate Deserialize(BinaryReader reader)
	{
		GameSettingsUpdate _ret = new GameSettingsUpdate();
	{
		_ret.values = new List<long>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			long element;
		element = (long)reader.ReadInt64();
			_ret.values.Add(element);
		}
	}
		return _ret;
	}
};
