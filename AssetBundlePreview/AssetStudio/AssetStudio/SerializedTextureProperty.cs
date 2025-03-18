using System.IO;

namespace AssetStudio;

public class SerializedTextureProperty
{
	public string m_DefaultName;

	public TextureDimension m_TexDim;

	public SerializedTextureProperty(BinaryReader reader)
	{
		m_DefaultName = reader.ReadAlignedString();
		m_TexDim = (TextureDimension)reader.ReadInt32();
	}
}
