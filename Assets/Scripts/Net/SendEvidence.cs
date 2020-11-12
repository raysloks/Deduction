using System.IO;
using System.Collections.Generic;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SendEvidence
{
	public List<Vector3> picturePos;
	public ulong id;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.picturePos.Count;
			writer.Write(size);
			foreach (var i in this.picturePos)
		{
			writer.Write(i.x);
			writer.Write(i.y);
			writer.Write(i.z);
		}
		}
		writer.Write((ulong)id);
	}

	public static SendEvidence Deserialize(BinaryReader reader)
	{
		SendEvidence _ret = new SendEvidence();
	{
		_ret.picturePos = new List<Vector3>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			Vector3 element;
		element = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			_ret.picturePos.Add(element);
		}
	}
		_ret.id = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
