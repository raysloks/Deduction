using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MobStateUpdate
{
	public MobUpdate update;
	public ulong type;
	public ulong state;
	public ulong sprite;
	public Vector3 color;

	public void Serialize(BinaryWriter writer)
	{
		update.Serialize(writer);
		writer.Write((ulong)type);
		writer.Write((ulong)state);
		writer.Write((ulong)sprite);
		{
			writer.Write(color.x);
			writer.Write(color.y);
			writer.Write(color.z);
		}
	}

	public static MobStateUpdate Deserialize(BinaryReader reader)
	{
		MobStateUpdate _ret = new MobStateUpdate();
		_ret.update = MobUpdate.Deserialize(reader);
		_ret.type = (ulong)reader.ReadUInt64();
		_ret.state = (ulong)reader.ReadUInt64();
		_ret.sprite = (ulong)reader.ReadUInt64();
		_ret.color = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
