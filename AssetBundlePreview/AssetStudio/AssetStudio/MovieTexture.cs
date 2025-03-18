namespace AssetStudio;

public sealed class MovieTexture : Texture
{
	public byte[] m_MovieData;

	public PPtr<AudioClip> m_AudioClip;

	public MovieTexture(ObjectReader reader)
		: base(reader)
	{
		reader.ReadBoolean();
		reader.AlignStream();
		m_AudioClip = new PPtr<AudioClip>(reader);
		m_MovieData = reader.ReadUInt8Array();
	}
}
