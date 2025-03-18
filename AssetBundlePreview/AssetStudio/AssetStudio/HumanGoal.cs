namespace AssetStudio;

public class HumanGoal
{
	public xform m_X;

	public float m_WeightT;

	public float m_WeightR;

	public Vector3 m_HintT;

	public float m_HintWeightT;

	public HumanGoal(ObjectReader reader)
	{
		int[] version = reader.version;
		m_X = new xform(reader);
		m_WeightT = reader.ReadSingle();
		m_WeightR = reader.ReadSingle();
		if (version[0] >= 5)
		{
			m_HintT = ((version[0] > 5 || (version[0] == 5 && version[1] >= 4)) ? reader.ReadVector3() : ((Vector3)reader.ReadVector4()));
			m_HintWeightT = reader.ReadSingle();
		}
	}
}
