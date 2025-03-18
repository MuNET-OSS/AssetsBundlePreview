namespace AssetStudio;

public class SerializedShader
{
	public SerializedProperties m_PropInfo;

	public SerializedSubShader[] m_SubShaders;

	public string[] m_KeywordNames;

	public byte[] m_KeywordFlags;

	public string m_Name;

	public string m_CustomEditorName;

	public string m_FallbackName;

	public SerializedShaderDependency[] m_Dependencies;

	public SerializedCustomEditorForRenderPipeline[] m_CustomEditorForRenderPipelines;

	public bool m_DisableNoSubshadersMessage;

	public SerializedShader(ObjectReader reader)
	{
		int[] version = reader.version;
		m_PropInfo = new SerializedProperties(reader);
		int numSubShaders = reader.ReadInt32();
		m_SubShaders = new SerializedSubShader[numSubShaders];
		for (int k = 0; k < numSubShaders; k++)
		{
			m_SubShaders[k] = new SerializedSubShader(reader);
		}
		if (version[0] > 2021 || (version[0] == 2021 && version[1] >= 2))
		{
			m_KeywordNames = reader.ReadStringArray();
			m_KeywordFlags = reader.ReadUInt8Array();
			reader.AlignStream();
		}
		m_Name = reader.ReadAlignedString();
		m_CustomEditorName = reader.ReadAlignedString();
		m_FallbackName = reader.ReadAlignedString();
		int numDependencies = reader.ReadInt32();
		m_Dependencies = new SerializedShaderDependency[numDependencies];
		for (int j = 0; j < numDependencies; j++)
		{
			m_Dependencies[j] = new SerializedShaderDependency(reader);
		}
		if (version[0] >= 2021)
		{
			int m_CustomEditorForRenderPipelinesSize = reader.ReadInt32();
			m_CustomEditorForRenderPipelines = new SerializedCustomEditorForRenderPipeline[m_CustomEditorForRenderPipelinesSize];
			for (int i = 0; i < m_CustomEditorForRenderPipelinesSize; i++)
			{
				m_CustomEditorForRenderPipelines[i] = new SerializedCustomEditorForRenderPipeline(reader);
			}
		}
		m_DisableNoSubshadersMessage = reader.ReadBoolean();
		reader.AlignStream();
	}
}
