using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SetMobColor
{
	public Vector3 color;

	public void Serialize(BinaryWriter writer)
	{
		{
			writer.Write(color.x);
			writer.Write(color.y);
			writer.Write(color.z);
		}
	}

	public static SetMobColor Deserialize(BinaryReader reader)
	{
		SetMobColor _ret = new SetMobColor();
		_ret.color = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};