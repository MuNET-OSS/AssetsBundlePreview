namespace AssetStudio;

public sealed class SkinnedMeshRenderer : Renderer
{
	public PPtr<Mesh> m_Mesh;

	public PPtr<Transform>[] m_Bones;

	public float[] m_BlendShapeWeights;

	public SkinnedMeshRenderer(ObjectReader reader)
		: base(reader)
	{
		reader.ReadInt32();
		reader.ReadBoolean();
		reader.ReadBoolean();
		reader.AlignStream();
		if (version[0] == 2 && version[1] < 6)
		{
			new PPtr<Animation>(reader);
		}
		m_Mesh = new PPtr<Mesh>(reader);
		m_Bones = new PPtr<Transform>[reader.ReadInt32()];
		for (int b = 0; b < m_Bones.Length; b++)
		{
			m_Bones[b] = new PPtr<Transform>(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_BlendShapeWeights = reader.ReadSingleArray();
		}
	}
}
