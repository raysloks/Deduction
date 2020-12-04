using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PickupCooldown
{
	public long child;
	public int random;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)child);
		writer.Write((int)random);
	}

	public static PickupCooldown Deserialize(BinaryReader reader)
	{
		PickupCooldown _ret = new PickupCooldown();
		_ret.child = (long)reader.ReadInt64();
		_ret.random = (int)reader.ReadInt32();
		return _ret;
	}
};
