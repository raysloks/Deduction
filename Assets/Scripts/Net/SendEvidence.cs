using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SendEvidence
{
	public List<PhotoPose> poses;
	public int photographer;
	public ulong index;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.poses.Count;
			writer.Write(size);
			foreach (var i in this.poses)
		i.Serialize(writer);
		}
		writer.Write((int)photographer);
		writer.Write((ulong)index);
	}

	public static SendEvidence Deserialize(BinaryReader reader)
	{
		SendEvidence _ret = new SendEvidence();
	{
		_ret.poses = new List<PhotoPose>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			PhotoPose element = new PhotoPose();
		element = PhotoPose.Deserialize(reader);
			_ret.poses.Add(element);
		}
	}
		_ret.photographer = (int)reader.ReadInt32();
		_ret.index = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
