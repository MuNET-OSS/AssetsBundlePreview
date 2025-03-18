namespace AssetStudio;

public class SerializedProgram
{
	public SerializedSubProgram[] m_SubPrograms;

	public SerializedProgramParameters m_CommonParameters;

	public ushort[] m_SerializedKeywordStateMask;

	public SerializedProgram(ObjectReader reader)
	{
		int[] version = reader.version;
		int numSubPrograms = reader.ReadInt32();
		m_SubPrograms = new SerializedSubProgram[numSubPrograms];
		for (int i = 0; i < numSubPrograms; i++)
		{
			m_SubPrograms[i] = new SerializedSubProgram(reader);
		}
		if ((version[0] == 2020 && version[1] > 3) || (version[0] == 2020 && version[1] == 3 && version[2] >= 2) || version[0] > 2021 || (version[0] == 2021 && version[1] > 1) || (version[0] == 2021 && version[1] == 1 && version[2] >= 1))
		{
			m_CommonParameters = new SerializedProgramParameters(reader);
		}
		if (version[0] > 2022 || (version[0] == 2022 && version[1] >= 1))
		{
			m_SerializedKeywordStateMask = reader.ReadUInt16Array();
			reader.AlignStream();
		}
	}
}
