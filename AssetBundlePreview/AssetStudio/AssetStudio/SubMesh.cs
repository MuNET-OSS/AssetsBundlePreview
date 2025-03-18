namespace AssetStudio;

public class SubMesh
{
	public uint firstByte;

	public uint indexCount;

	public GfxPrimitiveType topology;

	public uint triangleCount;

	public uint baseVertex;

	public uint firstVertex;

	public uint vertexCount;

	public AABB localAABB;

	public SubMesh(ObjectReader reader)
	{
		int[] version = reader.version;
		firstByte = reader.ReadUInt32();
		indexCount = reader.ReadUInt32();
		topology = (GfxPrimitiveType)reader.ReadInt32();
		if (version[0] < 4)
		{
			triangleCount = reader.ReadUInt32();
		}
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3))
		{
			baseVertex = reader.ReadUInt32();
		}
		if (version[0] >= 3)
		{
			firstVertex = reader.ReadUInt32();
			vertexCount = reader.ReadUInt32();
			localAABB = new AABB(reader);
		}
	}
}
