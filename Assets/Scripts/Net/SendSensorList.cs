using System.IO;
using System.Collections.Generic;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct SendSensorList
{
	public List<string> names;

	public void Serialize(BinaryWriter writer)
	{
		{
			ushort size = (ushort)this.names.Count;
			writer.Write(size);
			foreach (var i in this.names)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(i);
			ushort string_size = (ushort)bytes.Length;
			writer.Write(string_size);
			writer.Write(bytes);
		}
		}
	}

	public static SendSensorList Deserialize(BinaryReader reader)
	{
		SendSensorList _ret = new SendSensorList();
	{
		_ret.names = new List<string>();
		ushort size = reader.ReadUInt16();
		for (int i = 0; i < size; ++i)
		{
			string element;
		{
			ushort string_size = reader.ReadUInt16();
			byte[] bytes = reader.ReadBytes(string_size);
			element = System.Text.Encoding.UTF8.GetString(bytes);
		}
			_ret.names.Add(element);
		}
	}
		return _ret;
	}
};
