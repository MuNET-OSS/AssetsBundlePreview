namespace AssetStudio;

public class ConstantClip
{
	public float[] data;

	public ConstantClip(ObjectReader reader)
	{
		data = reader.ReadSingleArray();
	}
}
