namespace AssetStudio;

public class GLTextureSettings
{
	public int m_FilterMode;

	public int m_Aniso;

	public float m_MipBias;

	public int m_WrapMode;

	public GLTextureSettings(ObjectReader reader)
	{
		int[] version = reader.version;
		m_FilterMode = reader.ReadInt32();
		m_Aniso = reader.ReadInt32();
		m_MipBias = reader.ReadSingle();
		if (version[0] >= 2017)
		{
			m_WrapMode = reader.ReadInt32();
			reader.ReadInt32();
			reader.ReadInt32();
		}
		else
		{
			m_WrapMode = reader.ReadInt32();
		}
	}
}
