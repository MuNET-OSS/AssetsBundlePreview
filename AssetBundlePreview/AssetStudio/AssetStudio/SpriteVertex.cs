namespace AssetStudio;

public class SpriteVertex
{
	public Vector3 pos;

	public Vector2 uv;

	public SpriteVertex(ObjectReader reader)
	{
		int[] version = reader.version;
		pos = reader.ReadVector3();
		if (version[0] < 4 || (version[0] == 4 && version[1] <= 3))
		{
			uv = reader.ReadVector2();
		}
	}
}
