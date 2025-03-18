namespace AssetStudio;

public sealed class BuildSettings : Object
{
	public string m_Version;

	public BuildSettings(ObjectReader reader)
		: base(reader)
	{
		reader.ReadStringArray();
		reader.ReadBoolean();
		reader.ReadBoolean();
		reader.ReadBoolean();
		reader.ReadBoolean();
		m_Version = reader.ReadAlignedString();
	}
}
