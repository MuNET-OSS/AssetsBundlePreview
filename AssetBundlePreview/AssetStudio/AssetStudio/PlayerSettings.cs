namespace AssetStudio;

public sealed class PlayerSettings : Object
{
	public string companyName;

	public string productName;

	public PlayerSettings(ObjectReader reader)
		: base(reader)
	{
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 4))
		{
			reader.ReadBytes(16);
		}
		reader.ReadBoolean();
		reader.AlignStream();
		reader.ReadInt32();
		reader.ReadInt32();
		if (version[0] < 5 || (version[0] == 5 && version[1] < 3))
		{
			if (version[0] < 5)
			{
				reader.ReadInt32();
				if (version[0] > 4 || (version[0] == 4 && version[1] >= 6))
				{
					reader.ReadInt32();
				}
			}
			reader.ReadInt32();
		}
		else
		{
			reader.ReadBoolean();
			reader.AlignStream();
		}
		if (version[0] > 3 || (version[0] == 3 && version[1] >= 5))
		{
			reader.ReadInt32();
		}
		companyName = reader.ReadAlignedString();
		productName = reader.ReadAlignedString();
	}
}
