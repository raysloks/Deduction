using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GamePhaseUpdate
{
	public ulong phase;
	public long timer;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)phase);
		writer.Write((long)timer);
	}

	public static GamePhaseUpdate Deserialize(BinaryReader reader)
	{
		GamePhaseUpdate _ret = new GamePhaseUpdate();
		_ret.phase = (ulong)reader.ReadUInt64();
		_ret.timer = (long)reader.ReadInt64();
		return _ret;
	}
};
