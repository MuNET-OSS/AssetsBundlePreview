namespace AssetStudio;

public class ValueArray
{
	public bool[] m_BoolValues;

	public int[] m_IntValues;

	public float[] m_FloatValues;

	public Vector4[] m_VectorValues;

	public Vector3[] m_PositionValues;

	public Vector4[] m_QuaternionValues;

	public Vector3[] m_ScaleValues;

	public ValueArray(ObjectReader reader)
	{
		int[] version = reader.version;
		if (version[0] < 5 || (version[0] == 5 && version[1] < 5))
		{
			m_BoolValues = reader.ReadBooleanArray();
			reader.AlignStream();
			m_IntValues = reader.ReadInt32Array();
			m_FloatValues = reader.ReadSingleArray();
		}
		if (version[0] < 4 || (version[0] == 4 && version[1] < 3))
		{
			m_VectorValues = reader.ReadVector4Array();
			return;
		}
		int numPosValues = reader.ReadInt32();
		m_PositionValues = new Vector3[numPosValues];
		for (int j = 0; j < numPosValues; j++)
		{
			m_PositionValues[j] = ((version[0] > 5 || (version[0] == 5 && version[1] >= 4)) ? reader.ReadVector3() : ((Vector3)reader.ReadVector4()));
		}
		m_QuaternionValues = reader.ReadVector4Array();
		int numScaleValues = reader.ReadInt32();
		m_ScaleValues = new Vector3[numScaleValues];
		for (int i = 0; i < numScaleValues; i++)
		{
			m_ScaleValues[i] = ((version[0] > 5 || (version[0] == 5 && version[1] >= 4)) ? reader.ReadVector3() : ((Vector3)reader.ReadVector4()));
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 5))
		{
			m_FloatValues = reader.ReadSingleArray();
			m_IntValues = reader.ReadInt32Array();
			m_BoolValues = reader.ReadBooleanArray();
			reader.AlignStream();
		}
	}
}
