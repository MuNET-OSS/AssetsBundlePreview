namespace AssetStudio;

public sealed class Font : NamedObject
{
	public byte[] m_FontData;

	public Font(ObjectReader reader)
		: base(reader)
	{
		if ((version[0] == 5 && version[1] >= 5) || version[0] > 5)
		{
			reader.ReadSingle();
			new PPtr<Material>(reader);
			reader.ReadSingle();
			new PPtr<Texture>(reader);
			reader.ReadInt32();
			reader.ReadSingle();
			reader.ReadInt32();
			reader.ReadInt32();
			reader.ReadInt32();
			int m_CharacterRects_size = reader.ReadInt32();
			for (int j = 0; j < m_CharacterRects_size; j++)
			{
				reader.Position += 44L;
			}
			int m_KerningValues_size = reader.ReadInt32();
			for (int i = 0; i < m_KerningValues_size; i++)
			{
				reader.Position += 8L;
			}
			reader.ReadSingle();
			int m_FontData_size = reader.ReadInt32();
			if (m_FontData_size > 0)
			{
				m_FontData = reader.ReadBytes(m_FontData_size);
			}
			return;
		}
		reader.ReadInt32();
		if (version[0] <= 3)
		{
			reader.ReadInt32();
			reader.ReadInt32();
		}
		reader.ReadSingle();
		reader.ReadSingle();
		if (version[0] <= 3)
		{
			int m_PerCharacterKerning_size = reader.ReadInt32();
			for (int m = 0; m < m_PerCharacterKerning_size; m++)
			{
				reader.ReadInt32();
				reader.ReadSingle();
			}
		}
		else
		{
			reader.ReadInt32();
			reader.ReadInt32();
		}
		reader.ReadInt32();
		new PPtr<Material>(reader);
		int m_CharacterRects_size2 = reader.ReadInt32();
		for (int l = 0; l < m_CharacterRects_size2; l++)
		{
			reader.ReadInt32();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			reader.ReadSingle();
			if (version[0] >= 4)
			{
				reader.ReadBoolean();
				reader.AlignStream();
			}
		}
		new PPtr<Texture>(reader);
		int m_KerningValues_size2 = reader.ReadInt32();
		for (int k = 0; k < m_KerningValues_size2; k++)
		{
			reader.ReadInt16();
			reader.ReadInt16();
			reader.ReadSingle();
		}
		if (version[0] <= 3)
		{
			reader.ReadBoolean();
			reader.AlignStream();
		}
		else
		{
			reader.ReadSingle();
		}
		int m_FontData_size2 = reader.ReadInt32();
		if (m_FontData_size2 > 0)
		{
			m_FontData = reader.ReadBytes(m_FontData_size2);
		}
	}
}
