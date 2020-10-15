using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct LobbyRequest
{
	public string lobby;

	public void Serialize(BinaryWriter writer)
	{
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(lobby);
			ushort size = (ushort)bytes.Length;
			writer.Write(size);
			writer.Write(bytes);
		}
	}

	public static LobbyRequest Deserialize(BinaryReader reader)
	{
		LobbyRequest _ret = new LobbyRequest();
		{
			ushort size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(size);
			_ret.lobby = System.Text.Encoding.UTF8.GetString(bytes);
		}
		return _ret;
	}
};
