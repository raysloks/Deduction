using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameSettingSet
{
	public ushort setting;
	public long value;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ushort)setting);
		writer.Write((long)value);
	}

	public static GameSettingSet Deserialize(BinaryReader reader)
	{
		GameSettingSet _ret = new GameSettingSet();
		_ret.setting = (ushort)reader.ReadUInt16();
		_ret.value = (long)reader.ReadInt64();
		return _ret;
	}
};
