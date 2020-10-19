using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct TaskUpdate
{
	public ushort task;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ushort)task);
	}

	public static TaskUpdate Deserialize(BinaryReader reader)
	{
		TaskUpdate _ret = new TaskUpdate();
		_ret.task = (ushort)reader.ReadUInt16();
		return _ret;
	}
};
