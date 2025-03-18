namespace AssetStudio;

public class TextureParameter
{
	public int m_NameIndex;

	public int m_Index;

	public int m_SamplerIndex;

	public sbyte m_Dim;

	public TextureParameter(ObjectReader reader)
	{
		int[] version = reader.version;
		m_NameIndex = reader.ReadInt32();
		m_Index = reader.ReadInt32();
		m_SamplerIndex = reader.ReadInt32();
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3))
		{
			reader.ReadBoolean();
		}
		m_Dim = reader.ReadSByte();
		reader.AlignStream();
	}
}
