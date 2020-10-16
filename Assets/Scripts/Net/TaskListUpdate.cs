using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct TaskListUpdate
{
	public List<ushort> tasks;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.tasks.Count;
			writer.Write(size);
			foreach (var i in this.tasks)
		writer.Write((ushort)i);
		}
	}

	public static TaskListUpdate Deserialize(BinaryReader reader)
	{
		TaskListUpdate _ret = new TaskListUpdate();
	{
		_ret.tasks = new List<ushort>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			ushort element;
		element = (ushort)reader.ReadUInt16();
			_ret.tasks.Add(element);
		}
	}
		return _ret;
	}
};
