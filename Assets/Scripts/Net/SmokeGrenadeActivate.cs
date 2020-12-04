using System.IO;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SmokeGrenadeActivate
{
	public Vector3 pos;

	public void Serialize(BinaryWriter writer)
	{
		{
			writer.Write(pos.x);
			writer.Write(pos.y);
			writer.Write(pos.z);
		}
	}

	public static SmokeGrenadeActivate Deserialize(BinaryReader reader)
	{
		SmokeGrenadeActivate _ret = new SmokeGrenadeActivate();
		_ret.pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
