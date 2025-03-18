namespace AssetStudio;

public class SpriteRenderData
{
	public PPtr<Texture2D> texture;

	public PPtr<Texture2D> alphaTexture;

	public SecondarySpriteTexture[] secondaryTextures;

	public SubMesh[] m_SubMeshes;

	public byte[] m_IndexBuffer;

	public VertexData m_VertexData;

	public SpriteVertex[] vertices;

	public ushort[] indices;

	public Matrix4x4[] m_Bindpose;

	public BoneWeights4[] m_SourceSkin;

	public Rectf textureRect;

	public Vector2 textureRectOffset;

	public Vector2 atlasRectOffset;

	public SpriteSettings settingsRaw;

	public Vector4 uvTransform;

	public float downscaleMultiplier;

	public SpriteRenderData(ObjectReader reader)
	{
		int[] version = reader.version;
		texture = new PPtr<Texture2D>(reader);
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 2))
		{
			alphaTexture = new PPtr<Texture2D>(reader);
		}
		if (version[0] >= 2019)
		{
			int secondaryTexturesSize = reader.ReadInt32();
			secondaryTextures = new SecondarySpriteTexture[secondaryTexturesSize];
			for (int l = 0; l < secondaryTexturesSize; l++)
			{
				secondaryTextures[l] = new SecondarySpriteTexture(reader);
			}
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 6))
		{
			int m_SubMeshesSize = reader.ReadInt32();
			m_SubMeshes = new SubMesh[m_SubMeshesSize];
			for (int j = 0; j < m_SubMeshesSize; j++)
			{
				m_SubMeshes[j] = new SubMesh(reader);
			}
			m_IndexBuffer = reader.ReadUInt8Array();
			reader.AlignStream();
			m_VertexData = new VertexData(reader);
		}
		else
		{
			int verticesSize = reader.ReadInt32();
			vertices = new SpriteVertex[verticesSize];
			for (int k = 0; k < verticesSize; k++)
			{
				vertices[k] = new SpriteVertex(reader);
			}
			indices = reader.ReadUInt16Array();
			reader.AlignStream();
		}
		if (version[0] >= 2018)
		{
			m_Bindpose = reader.ReadMatrixArray();
			if (version[0] == 2018 && version[1] < 2)
			{
				int m_SourceSkinSize = reader.ReadInt32();
				for (int i = 0; i < m_SourceSkinSize; i++)
				{
					m_SourceSkin[i] = new BoneWeights4(reader);
				}
			}
		}
		textureRect = new Rectf(reader);
		textureRectOffset = reader.ReadVector2();
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 6))
		{
			atlasRectOffset = reader.ReadVector2();
		}
		settingsRaw = new SpriteSettings(reader);
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 5))
		{
			uvTransform = reader.ReadVector4();
		}
		if (version[0] >= 2017)
		{
			downscaleMultiplier = reader.ReadSingle();
		}
	}
}
