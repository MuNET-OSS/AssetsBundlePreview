namespace AssetStudio;

public class SerializedProgramParameters
{
	public VectorParameter[] m_VectorParams;

	public MatrixParameter[] m_MatrixParams;

	public TextureParameter[] m_TextureParams;

	public BufferBinding[] m_BufferParams;

	public ConstantBuffer[] m_ConstantBuffers;

	public BufferBinding[] m_ConstantBufferBindings;

	public UAVParameter[] m_UAVParams;

	public SamplerParameter[] m_Samplers;

	public SerializedProgramParameters(ObjectReader reader)
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
		int numSamplers = reader.ReadInt32();
		m_Samplers = new SamplerParameter[numSamplers];
		for (int i = 0; i < numSamplers; i++)
		{
			m_Samplers[i] = new SamplerParameter(reader);
		}
	}
}
