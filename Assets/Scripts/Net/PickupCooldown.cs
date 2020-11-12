using System.IO;

// WARNING : Auto-generated file, changes made will disappear when re-generated.

public struct PickupCooldown
{
	public long child;

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((long)child);
	}

	public static PickupCooldown Deserialize(BinaryReader reader)
	{
		PickupCooldown _ret = new PickupCooldown();
		_ret.child = (long)reader.ReadInt64();
		return _ret;
	}
};
