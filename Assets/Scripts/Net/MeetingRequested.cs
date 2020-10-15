using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct MeetingRequested
{
	public ulong idOfInitiator;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)idOfInitiator);
	}

	public static MeetingRequested Deserialize(BinaryReader reader)
	{
		MeetingRequested _ret = new MeetingRequested();
		_ret.idOfInitiator = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
