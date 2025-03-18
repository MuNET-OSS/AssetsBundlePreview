namespace AssetStudio;

public sealed class Texture2D : Texture
{
	public int m_Width;

	public int m_Height;

	public TextureFormat m_TextureFormat;

	public bool m_MipMap;

	public int m_MipCount;

	public GLTextureSettings m_TextureSettings;

	public ResourceReader image_data;

	public StreamingInfo m_StreamData;

	public Texture2D(ObjectReader reader)
		: base(reader)
	{
		m_Width = reader.ReadInt32();
		m_Height = reader.ReadInt32();
		reader.ReadInt32();
		if (version[0] >= 2020)
		{
			reader.ReadInt32();
		}
		m_TextureFormat = (TextureFormat)reader.ReadInt32();
		if (version[0] < 5 || (version[0] == 5 && version[1] < 2))
		{
			m_MipMap = reader.ReadBoolean();
		}
		else
		{
			m_MipCount = reader.ReadInt32();
		}
		if (version[0] > 2 || (version[0] == 2 && version[1] >= 6))
		{
			reader.ReadBoolean();
		}
		if (version[0] >= 2020)
		{
			reader.ReadBoolean();
		}
		if (version[0] > 2019 || (version[0] == 2019 && version[1] >= 3))
		{
			reader.ReadBoolean();
		}
		if (version[0] >= 3 && (version[0] < 5 || (version[0] == 5 && version[1] <= 4)))
		{
			reader.ReadBoolean();
		}
		if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 2))
		{
			reader.ReadBoolean();
		}
		reader.AlignStream();
		if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 2))
		{
			reader.ReadInt32();
		}
		reader.ReadInt32();
		reader.ReadInt32();
		m_TextureSettings = new GLTextureSettings(reader);
		if (version[0] >= 3)
		{
			reader.ReadInt32();
		}
		if (version[0] > 3 || (version[0] == 3 && version[1] >= 5))
		{
			reader.ReadInt32();
		}
		if (version[0] > 2020 || (version[0] == 2020 && version[1] >= 2))
		{
			reader.ReadUInt8Array();
			reader.AlignStream();
		}
		int image_data_size = reader.ReadInt32();
		if (image_data_size == 0 && ((version[0] == 5 && version[1] >= 3) || version[0] > 5))
		{
			m_StreamData = new StreamingInfo(reader);
		}
		image_data = (string.IsNullOrEmpty(m_StreamData?.path) ? new ResourceReader(reader, reader.BaseStream.Position, image_data_size) : new ResourceReader(m_StreamData.path, assetsFile, m_StreamData.offset, m_StreamData.size));
	}
}
