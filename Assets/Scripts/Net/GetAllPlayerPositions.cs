using System.IO;
using System.Collections.Generic;
using UnityEngine;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GetAllPlayerPositions
{
	public List<Vector3> playerPos;
	public ulong id;
	public Vector3 player;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.playerPos.Count;
			writer.Write(size);
			foreach (var i in this.playerPos)
		{
			writer.Write(i.x);
			writer.Write(i.y);
			writer.Write(i.z);
		}
		}
		writer.Write((ulong)id);
		{
			writer.Write(player.x);
			writer.Write(player.y);
			writer.Write(player.z);
		}
	}

	public static GetAllPlayerPositions Deserialize(BinaryReader reader)
	{
		GetAllPlayerPositions _ret = new GetAllPlayerPositions();
	{
		_ret.playerPos = new List<Vector3>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			Vector3 element;
		element = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			_ret.playerPos.Add(element);
		}
	}
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.player = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		return _ret;
	}
};
