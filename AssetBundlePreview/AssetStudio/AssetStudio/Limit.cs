namespace AssetStudio;

public class Limit
{
	public object m_Min;

	public object m_Max;

	public Limit(ObjectReader reader)
	{
		int[] version = reader.version;
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 4))
		{
			m_Min = reader.ReadVector3();
			m_Max = reader.ReadVector3();
		}
		else
		{
			m_Min = reader.ReadVector4();
			m_Max = reader.ReadVector4();
		}
	}
}
