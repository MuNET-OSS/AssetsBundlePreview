namespace AssetStudio;

public class BlendShapeData
{
	public BlendShapeVertex[] vertices;

	public MeshBlendShape[] shapes;

	public MeshBlendShapeChannel[] channels;

	public float[] fullWeights;

	public BlendShapeData(ObjectReader reader)
	{
		int[] version = reader.version;
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			int numVerts = reader.ReadInt32();
			vertices = new BlendShapeVertex[numVerts];
			for (int k = 0; k < numVerts; k++)
			{
				vertices[k] = new BlendShapeVertex(reader);
			}
			int numShapes = reader.ReadInt32();
			shapes = new MeshBlendShape[numShapes];
			for (int j = 0; j < numShapes; j++)
			{
				shapes[j] = new MeshBlendShape(reader);
			}
			int numChannels = reader.ReadInt32();
			channels = new MeshBlendShapeChannel[numChannels];
			for (int i = 0; i < numChannels; i++)
			{
				channels[i] = new MeshBlendShapeChannel(reader);
			}
			fullWeights = reader.ReadSingleArray();
		}
		else
		{
			int m_ShapesSize = reader.ReadInt32();
			MeshBlendShape[] m_Shapes = new MeshBlendShape[m_ShapesSize];
			for (int m = 0; m < m_ShapesSize; m++)
			{
				m_Shapes[m] = new MeshBlendShape(reader);
			}
			reader.AlignStream();
			int m_ShapeVerticesSize = reader.ReadInt32();
			BlendShapeVertex[] m_ShapeVertices = new BlendShapeVertex[m_ShapeVerticesSize];
			for (int l = 0; l < m_ShapeVerticesSize; l++)
			{
				m_ShapeVertices[l] = new BlendShapeVertex(reader);
			}
		}
	}
}
