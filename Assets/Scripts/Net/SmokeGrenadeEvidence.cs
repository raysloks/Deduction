using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SmokeGrenadeEvidence
{
	public string area;
	public string playerName;
	public ulong playerId;

	public void Serialize(BinaryWriter writer)
	{
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(area);
			ushort string_size = (ushort)bytes.Length;
			writer.Write(string_size);
			writer.Write(bytes);
		}
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(playerName);
			ushort string_size = (ushort)bytes.Length;
			writer.Write(string_size);
			writer.Write(bytes);
		}
		writer.Write((ulong)playerId);
	}

	public static SmokeGrenadeEvidence Deserialize(BinaryReader reader)
	{
		SmokeGrenadeEvidence _ret = new SmokeGrenadeEvidence();
		{
			ushort string_size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(string_size);
			_ret.area = System.Text.Encoding.UTF8.GetString(bytes);
		}
		{
			ushort string_size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(string_size);
			_ret.playerName = System.Text.Encoding.UTF8.GetString(bytes);
		}
		_ret.playerId = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
