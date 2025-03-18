namespace AssetStudio;

public class SerializedSubProgram
{
	public uint m_BlobIndex;

	public ParserBindChannels m_Channels;

	public ushort[] m_KeywordIndices;

	public sbyte m_ShaderHardwareTier;

	public ShaderGpuProgramType m_GpuProgramType;

	public SerializedProgramParameters m_Parameters;

	public VectorParameter[] m_VectorParams;

	public MatrixParameter[] m_MatrixParams;

	public TextureParameter[] m_TextureParams;

	public BufferBinding[] m_BufferParams;

	public ConstantBuffer[] m_ConstantBuffers;

	public BufferBinding[] m_ConstantBufferBindings;

	public UAVParameter[] m_UAVParams;

	public SamplerParameter[] m_Samplers;

	public SerializedSubProgram(ObjectReader reader)
	{
		int[] version = reader.version;
		m_BlobIndex = reader.ReadUInt32();
		m_Channels = new ParserBindChannels(reader);
		if ((version[0] >= 2019 && version[0] < 2021) || (version[0] == 2021 && version[1] < 2))
		{
			reader.ReadUInt16Array();
			reader.AlignStream();
			reader.ReadUInt16Array();
			reader.AlignStream();
		}
		else
		{
			m_KeywordIndices = reader.ReadUInt16Array();
			if (version[0] >= 2017)
			{
				reader.AlignStream();
			}
		}
		m_ShaderHardwareTier = reader.ReadSByte();
		m_GpuProgramType = (ShaderGpuProgramType)reader.ReadSByte();
		reader.AlignStream();
		if ((version[0] == 2020 && version[1] > 3) || (version[0] == 2020 && version[1] == 3 && version[2] >= 2) || version[0] > 2021 || (version[0] == 2021 && version[1] > 1) || (version[0] == 2021 && version[1] == 1 && version[2] >= 1))
		{
			m_Parameters = new SerializedProgramParameters(reader);
		}
		else
		{
			int numVectorParams = reader.ReadInt32();
			m_VectorParams = new VectorParameter[numVectorParams];
			for (int i3 = 0; i3 < numVectorParams; i3++)
			{
				m_VectorParams[i3] = new VectorParameter(reader);
			}
			int numMatrixParams = reader.ReadInt32();
			m_MatrixParams = new MatrixParameter[numMatrixParams];
			for (int i2 = 0; i2 < numMatrixParams; i2++)
			{
				m_MatrixParams[i2] = new MatrixParameter(reader);
			}
			int numTextureParams = reader.ReadInt32();
			m_TextureParams = new TextureParameter[numTextureParams];
			for (int n = 0; n < numTextureParams; n++)
			{
				m_TextureParams[n] = new TextureParameter(reader);
			}
			int numBufferParams = reader.ReadInt32();
			m_BufferParams = new BufferBinding[numBufferParams];
			for (int m = 0; m < numBufferParams; m++)
			{
				m_BufferParams[m] = new BufferBinding(reader);
			}
			int numConstantBuffers = reader.ReadInt32();
			m_ConstantBuffers = new ConstantBuffer[numConstantBuffers];
			for (int l = 0; l < numConstantBuffers; l++)
			{
				m_ConstantBuffers[l] = new ConstantBuffer(reader);
			}
			int numConstantBufferBindings = reader.ReadInt32();
			m_ConstantBufferBindings = new BufferBinding[numConstantBufferBindings];
			for (int k = 0; k < numConstantBufferBindings; k++)
			{
				m_ConstantBufferBindings[k] = new BufferBinding(reader);
			}
			int numUAVParams = reader.ReadInt32();
			m_UAVParams = new UAVParameter[numUAVParams];
			for (int j = 0; j < numUAVParams; j++)
			{
				m_UAVParams[j] = new UAVParameter(reader);
			}
			if (version[0] >= 2017)
			{
				int numSamplers = reader.ReadInt32();
				m_Samplers = new SamplerParameter[numSamplers];
				for (int i = 0; i < numSamplers; i++)
				{
					m_Samplers[i] = new SamplerParameter(reader);
				}
			}
		}
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2))
		{
			if (version[0] >= 2021)
			{
				reader.ReadInt64();
			}
			else
			{
				reader.ReadInt32();
			}
		}
	}
}
