using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobUpdate
{
	public ulong id;
	public long tick;
	public Vector3 position;
	public Vector3 velocity;
	public Vector3 facing;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((long)tick);
		writer.Write(position.x);
		writer.Write(position.y);
		writer.Write(position.z);
		writer.Write(velocity.x);
		writer.Write(velocity.y);
		writer.Write(velocity.z);
		writer.Write(facing.x);
		writer.Write(facing.y);
		writer.Write(facing.z);
	}

	public static MobUpdate Deserialize(BinaryReader reader)
	{
		MobUpdate _ret = new MobUpdate();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.tick = (long)reader.ReadInt64();
		_ret.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		_ret.velocity = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		_ret.facing = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
