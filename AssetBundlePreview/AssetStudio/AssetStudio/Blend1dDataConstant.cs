namespace AssetStudio;

public class Blend1dDataConstant
{
	public float[] m_ChildThresholdArray;

	public Blend1dDataConstant(ObjectReader reader)
	{
		m_ChildThresholdArray = reader.ReadSingleArray();
	}
}
