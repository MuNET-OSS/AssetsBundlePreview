namespace AssetStudio;

public sealed class VideoClip : NamedObject
{
	public ResourceReader m_VideoData;

	public string m_OriginalPath;

	public StreamedResource m_ExternalResources;

	public VideoClip(ObjectReader reader)
		: base(reader)
	{
		m_OriginalPath = reader.ReadAlignedString();
		reader.ReadUInt32();
		reader.ReadUInt32();
		reader.ReadUInt32();
		reader.ReadUInt32();
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2))
		{
			reader.ReadUInt32();
			reader.ReadUInt32();
		}
		reader.ReadDouble();
		reader.ReadUInt64();
		reader.ReadInt32();
		reader.ReadUInt16Array();
		reader.AlignStream();
		reader.ReadUInt32Array();
		reader.ReadStringArray();
		if (version[0] >= 2020)
		{
			int m_VideoShadersSize = reader.ReadInt32();
			PPtr<Shader>[] m_VideoShaders = new PPtr<Shader>[m_VideoShadersSize];
			for (int i = 0; i < m_VideoShadersSize; i++)
			{
				m_VideoShaders[i] = new PPtr<Shader>(reader);
			}
		}
		m_ExternalResources = new StreamedResource(reader);
		reader.ReadBoolean();
		if (version[0] >= 2020)
		{
			reader.ReadBoolean();
		}
		m_VideoData = (string.IsNullOrEmpty(m_ExternalResources.m_Source) ? new ResourceReader(reader, reader.BaseStream.Position, m_ExternalResources.m_Size) : new ResourceReader(m_ExternalResources.m_Source, assetsFile, m_ExternalResources.m_Offset, m_ExternalResources.m_Size));
	}
}
