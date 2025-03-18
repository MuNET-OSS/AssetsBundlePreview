namespace AssetStudio;

public sealed class Material : NamedObject
{
	public PPtr<Shader> m_Shader;

	public UnityPropertySheet m_SavedProperties;

	public Material(ObjectReader reader)
		: base(reader)
	{
		m_Shader = new PPtr<Shader>(reader);
		if (version[0] == 4 && version[1] >= 1)
		{
			reader.ReadStringArray();
		}
		if (version[0] > 2021 || (version[0] == 2021 && version[1] >= 3))
		{
			reader.ReadStringArray();
			reader.ReadStringArray();
		}
		else if (version[0] >= 5)
		{
			reader.ReadAlignedString();
		}
		if (version[0] >= 5)
		{
			reader.ReadUInt32();
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 6))
		{
			reader.ReadBoolean();
			reader.AlignStream();
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			reader.ReadInt32();
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 1))
		{
			int stringTagMapSize = reader.ReadInt32();
			for (int i = 0; i < stringTagMapSize; i++)
			{
				reader.ReadAlignedString();
				reader.ReadAlignedString();
			}
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 6))
		{
			reader.ReadStringArray();
		}
		m_SavedProperties = new UnityPropertySheet(reader);
	}
}
