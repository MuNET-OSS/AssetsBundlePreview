namespace AssetStudio;

public abstract class Texture : NamedObject
{
	protected Texture(ObjectReader reader)
		: base(reader)
	{
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3))
		{
			reader.ReadInt32();
			reader.ReadBoolean();
			if (version[0] > 2020 || (version[0] == 2020 && version[1] >= 2))
			{
				reader.ReadBoolean();
			}
			reader.AlignStream();
		}
	}
}
