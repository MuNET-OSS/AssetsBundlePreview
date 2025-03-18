namespace AssetStudio;

public class StateConstant
{
	public TransitionConstant[] m_TransitionConstantArray;

	public int[] m_BlendTreeConstantIndexArray;

	public LeafInfoConstant[] m_LeafInfoArray;

	public BlendTreeConstant[] m_BlendTreeConstantArray;

	public uint m_NameID;

	public uint m_PathID;

	public uint m_FullPathID;

	public uint m_TagID;

	public uint m_SpeedParamID;

	public uint m_MirrorParamID;

	public uint m_CycleOffsetParamID;

	public float m_Speed;

	public float m_CycleOffset;

	public bool m_IKOnFeet;

	public bool m_WriteDefaultValues;

	public bool m_Loop;

	public bool m_Mirror;

	public StateConstant(ObjectReader reader)
	{
		int[] version = reader.version;
		int numTransistions = reader.ReadInt32();
		m_TransitionConstantArray = new TransitionConstant[numTransistions];
		for (int k = 0; k < numTransistions; k++)
		{
			m_TransitionConstantArray[k] = new TransitionConstant(reader);
		}
		m_BlendTreeConstantIndexArray = reader.ReadInt32Array();
		if (version[0] < 5 || (version[0] == 5 && version[1] < 2))
		{
			int numInfos = reader.ReadInt32();
			m_LeafInfoArray = new LeafInfoConstant[numInfos];
			for (int j = 0; j < numInfos; j++)
			{
				m_LeafInfoArray[j] = new LeafInfoConstant(reader);
			}
		}
		int numBlends = reader.ReadInt32();
		m_BlendTreeConstantArray = new BlendTreeConstant[numBlends];
		for (int i = 0; i < numBlends; i++)
		{
			m_BlendTreeConstantArray[i] = new BlendTreeConstant(reader);
		}
		m_NameID = reader.ReadUInt32();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_PathID = reader.ReadUInt32();
		}
		if (version[0] >= 5)
		{
			m_FullPathID = reader.ReadUInt32();
		}
		m_TagID = reader.ReadUInt32();
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 1))
		{
			m_SpeedParamID = reader.ReadUInt32();
			m_MirrorParamID = reader.ReadUInt32();
			m_CycleOffsetParamID = reader.ReadUInt32();
		}
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2))
		{
			reader.ReadUInt32();
		}
		m_Speed = reader.ReadSingle();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 1))
		{
			m_CycleOffset = reader.ReadSingle();
		}
		m_IKOnFeet = reader.ReadBoolean();
		if (version[0] >= 5)
		{
			m_WriteDefaultValues = reader.ReadBoolean();
		}
		m_Loop = reader.ReadBoolean();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 1))
		{
			m_Mirror = reader.ReadBoolean();
		}
		reader.AlignStream();
	}
}
