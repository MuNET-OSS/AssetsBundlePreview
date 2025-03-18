namespace AssetStudio;

public class ImportedVertex
{
	public Vector3 Vertex { get; set; }

	public Vector3 Normal { get; set; }

	public float[][] UV { get; set; }

	public Vector4 Tangent { get; set; }

	public Color Color { get; set; }

	public float[] Weights { get; set; }

	public int[] BoneIndices { get; set; }
}
