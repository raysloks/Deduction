using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SabotageTaskUpdate
{
	public ushort sabotage;
	public ushort minigame_index;
	public long timer;
	public bool completed;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ushort)sabotage);
		writer.Write((ushort)minigame_index);
		writer.Write((long)timer);
		writer.Write((bool)completed);
	}

	public static SabotageTaskUpdate Deserialize(BinaryReader reader)
	{
		SabotageTaskUpdate _ret = new SabotageTaskUpdate();
		_ret.sabotage = (ushort)reader.ReadUInt16();
		_ret.minigame_index = (ushort)reader.ReadUInt16();
		_ret.timer = (long)reader.ReadInt64();
		_ret.completed = (bool)reader.ReadBoolean();
		return _ret;
	}
};
