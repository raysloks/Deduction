using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobUpdate
{
	public ulong id;
	public long time;
	public Vector3 position;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((long)time);
		{
			writer.Write(position.x);
			writer.Write(position.y);
			writer.Write(position.z);
		}
	}

	public static MobUpdate Deserialize(BinaryReader reader)
	{
		MobUpdate _ret = new MobUpdate();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.time = (long)reader.ReadInt64();
		_ret.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
