using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PhotoPose
{
	public ulong index;
	public Vector3 position;
	public bool flipped;
	public bool moving;
	public bool dead;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)index);
		{
			writer.Write(position.x);
			writer.Write(position.y);
			writer.Write(position.z);
		}
		writer.Write((bool)flipped);
		writer.Write((bool)moving);
		writer.Write((bool)dead);
	}

	public static PhotoPose Deserialize(BinaryReader reader)
	{
		PhotoPose _ret = new PhotoPose();
		_ret.index = (ulong)reader.ReadUInt64();
		_ret.position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		_ret.flipped = (bool)reader.ReadBoolean();
		_ret.moving = (bool)reader.ReadBoolean();
		_ret.dead = (bool)reader.ReadBoolean();
		return _ret;
	}
};
