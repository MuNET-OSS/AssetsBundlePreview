using System.IO;

namespace AssetStudio;

public class StructParameter
{
	public MatrixParameter[] m_MatrixParams;

	public VectorParameter[] m_VectorParams;

	public StructParameter(BinaryReader reader)
	{
		reader.ReadInt32();
		reader.ReadInt32();
		reader.ReadInt32();
		reader.ReadInt32();
		int numVectorParams = reader.ReadInt32();
		m_VectorParams = new VectorParameter[numVectorParams];
		for (int j = 0; j < numVectorParams; j++)
		{
			m_VectorParams[j] = new VectorParameter(reader);
		}
		int numMatrixParams = reader.ReadInt32();
		m_MatrixParams = new MatrixParameter[numMatrixParams];
		for (int i = 0; i < numMatrixParams; i++)
		{
			m_MatrixParams[i] = new MatrixParameter(reader);
		}
	}
}
