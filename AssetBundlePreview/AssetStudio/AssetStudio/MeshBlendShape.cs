namespace AssetStudio;

public class MeshBlendShape
{
	public uint firstVertex;

	public uint vertexCount;

	public bool hasNormals;

	public bool hasTangents;

	public MeshBlendShape(ObjectReader reader)
	{
		int[] version = reader.version;
		if (version[0] == 4 && version[1] < 3)
		{
			reader.ReadAlignedString();
		}
		firstVertex = reader.ReadUInt32();
		vertexCount = reader.ReadUInt32();
		if (version[0] == 4 && version[1] < 3)
		{
			reader.ReadVector3();
			reader.ReadVector3();
		}
		hasNormals = reader.ReadBoolean();
		hasTangents = reader.ReadBoolean();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			reader.AlignStream();
		}
	}
}
