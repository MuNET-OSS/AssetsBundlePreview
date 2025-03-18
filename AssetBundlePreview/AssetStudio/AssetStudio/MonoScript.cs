namespace AssetStudio;

public sealed class MonoScript : NamedObject
{
	public string m_ClassName;

	public string m_Namespace;

	public string m_AssemblyName;

	public MonoScript(ObjectReader reader)
		: base(reader)
	{
		if (version[0] > 3 || (version[0] == 3 && version[1] >= 4))
		{
			reader.ReadInt32();
		}
		if (version[0] < 5)
		{
			reader.ReadUInt32();
		}
		else
		{
			reader.ReadBytes(16);
		}
		if (version[0] < 3)
		{
			reader.ReadAlignedString();
		}
		m_ClassName = reader.ReadAlignedString();
		if (version[0] >= 3)
		{
			m_Namespace = reader.ReadAlignedString();
		}
		m_AssemblyName = reader.ReadAlignedString();
		if (version[0] < 2018 || (version[0] == 2018 && version[1] < 2))
		{
			reader.ReadBoolean();
		}
	}
}
