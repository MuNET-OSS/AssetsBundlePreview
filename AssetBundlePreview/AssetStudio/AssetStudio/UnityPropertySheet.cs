using System.Collections.Generic;

namespace AssetStudio;

public class UnityPropertySheet
{
	public KeyValuePair<string, UnityTexEnv>[] m_TexEnvs;

	public KeyValuePair<string, int>[] m_Ints;

	public KeyValuePair<string, float>[] m_Floats;

	public KeyValuePair<string, Color>[] m_Colors;

	public UnityPropertySheet(ObjectReader reader)
	{
		int[] version = reader.version;
		int m_TexEnvsSize = reader.ReadInt32();
		m_TexEnvs = new KeyValuePair<string, UnityTexEnv>[m_TexEnvsSize];
		for (int l = 0; l < m_TexEnvsSize; l++)
		{
			m_TexEnvs[l] = new KeyValuePair<string, UnityTexEnv>(reader.ReadAlignedString(), new UnityTexEnv(reader));
		}
		if (version[0] >= 2021)
		{
			int m_IntsSize = reader.ReadInt32();
			m_Ints = new KeyValuePair<string, int>[m_IntsSize];
			for (int k = 0; k < m_IntsSize; k++)
			{
				m_Ints[k] = new KeyValuePair<string, int>(reader.ReadAlignedString(), reader.ReadInt32());
			}
		}
		int m_FloatsSize = reader.ReadInt32();
		m_Floats = new KeyValuePair<string, float>[m_FloatsSize];
		for (int j = 0; j < m_FloatsSize; j++)
		{
			m_Floats[j] = new KeyValuePair<string, float>(reader.ReadAlignedString(), reader.ReadSingle());
		}
		int m_ColorsSize = reader.ReadInt32();
		m_Colors = new KeyValuePair<string, Color>[m_ColorsSize];
		for (int i = 0; i < m_ColorsSize; i++)
		{
			m_Colors[i] = new KeyValuePair<string, Color>(reader.ReadAlignedString(), reader.ReadColor4());
		}
	}
}
