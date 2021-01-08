using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PulseEvidence
{
	public int deadTime;
	public ulong playerId;
	public ulong deadId;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((int)deadTime);
		writer.Write((ulong)playerId);
		writer.Write((ulong)deadId);
	}

	public static PulseEvidence Deserialize(BinaryReader reader)
	{
		PulseEvidence _ret = new PulseEvidence();
		_ret.deadTime = (int)reader.ReadInt32();
		_ret.playerId = (ulong)reader.ReadUInt64();
		_ret.deadId = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
