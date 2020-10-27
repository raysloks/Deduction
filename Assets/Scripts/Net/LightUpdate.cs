using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct LightUpdate
{
	public long time;
	public float light;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)time);
		writer.Write((float)light);
	}

	public static LightUpdate Deserialize(BinaryReader reader)
	{
		LightUpdate _ret = new LightUpdate();
		_ret.time = (long)reader.ReadInt64();
		_ret.light = (float)reader.ReadSingle();
		return _ret;
	}
};
