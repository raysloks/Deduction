using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct GameStartRequested
{
	public string password;
	public ulong passwordLocation;

	public void Serialize(BinaryWriter writer)
	{
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
			ushort size = (ushort)bytes.Length;
			writer.Write(size);
			writer.Write(bytes);
		}
		writer.Write((ulong)passwordLocation);
	}

	public static GameStartRequested Deserialize(BinaryReader reader)
	{
		GameStartRequested _ret = new GameStartRequested();
		{
			ushort size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(size);
			_ret.password = System.Text.Encoding.UTF8.GetString(bytes);
		}
		_ret.passwordLocation = (ulong)reader.ReadUInt64();
		return _ret;
	}
};
