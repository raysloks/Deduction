using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct LobbyIdentity
{
	public string lobby;
	public string address;
	public int port;

	public void Serialize(BinaryWriter writer)
	{
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(lobby);
			ushort size = (ushort)bytes.Length;
			writer.Write(size);
			writer.Write(bytes);
		}
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(address);
			ushort size = (ushort)bytes.Length;
			writer.Write(size);
			writer.Write(bytes);
		}
		writer.Write((int)port);
	}

	public static LobbyIdentity Deserialize(BinaryReader reader)
	{
		LobbyIdentity _ret = new LobbyIdentity();
		{
			ushort size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(size);
			_ret.lobby = System.Text.Encoding.UTF8.GetString(bytes);
		}
		{
			ushort size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(size);
			_ret.address = System.Text.Encoding.UTF8.GetString(bytes);
		}
		_ret.port = (int)reader.ReadInt32();
		return _ret;
	}
};
