using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GivenTasks
{
	public List<byte> taskId;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.taskId.Count;
			writer.Write(size);
			foreach (var i in this.taskId)
		writer.Write((byte)i);
		}
	}

	public static GivenTasks Deserialize(BinaryReader reader)
	{
		GivenTasks _ret = new GivenTasks();
	{
		_ret.taskId = new List<byte>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			byte element;
		element = (byte)reader.ReadByte();
			_ret.taskId.Add(element);
		}
	}
		return _ret;
	}
};
