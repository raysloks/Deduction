using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobTeleport
{
	public ulong id;
	public long time;
	public Vector3 from;
	public Vector3 to;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((long)time);
		{
			writer.Write(from.x);
			writer.Write(from.y);
			writer.Write(from.z);
		}
		{
			writer.Write(to.x);
			writer.Write(to.y);
			writer.Write(to.z);
		}
	}

	public static MobTeleport Deserialize(BinaryReader reader)
	{
		MobTeleport _ret = new MobTeleport();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.time = (long)reader.ReadInt64();
		_ret.from = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		_ret.to = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
