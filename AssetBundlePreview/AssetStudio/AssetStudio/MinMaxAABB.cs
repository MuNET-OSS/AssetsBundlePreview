using System.IO;

namespace AssetStudio;

public class MinMaxAABB
{
	public Vector3 m_Min;

	public Vector3 m_Max;

	public MinMaxAABB(BinaryReader reader)
	{
		m_Min = reader.ReadVector3();
		m_Max = reader.ReadVector3();
	}
}
