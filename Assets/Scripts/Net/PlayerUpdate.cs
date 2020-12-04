using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PlayerUpdate
{
	public ulong id;
	public ulong mob;
	public string name;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((ulong)id);
		writer.Write((ulong)mob);
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(name);
			ushort string_size = (ushort)bytes.Length;
			writer.Write(string_size);
			writer.Write(bytes);
		}
	}

	public static PlayerUpdate Deserialize(BinaryReader reader)
	{
		PlayerUpdate _ret = new PlayerUpdate();
		_ret.id = (ulong)reader.ReadUInt64();
		_ret.mob = (ulong)reader.ReadUInt64();
		{
			ushort string_size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(string_size);
			_ret.name = System.Text.Encoding.UTF8.GetString(bytes);
		}
		return _ret;
	}
};
