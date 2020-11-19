using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PresentEvidence
{
	public ulong index;
	public ulong presenter;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)index);
		writer.Write((ulong)presenter);
	}

	public static PresentEvidence Deserialize(BinaryReader reader)
	{
		PresentEvidence _ret = new PresentEvidence();
		_ret.index = (ulong)reader.ReadUInt64();
		_ret.presenter = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
