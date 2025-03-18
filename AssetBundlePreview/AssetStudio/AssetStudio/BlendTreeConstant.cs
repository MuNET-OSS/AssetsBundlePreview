namespace AssetStudio;

public class BlendTreeConstant
{
	public BlendTreeNodeConstant[] m_NodeArray;

	public ValueArrayConstant m_BlendEventArrayConstant;

	public BlendTreeConstant(ObjectReader reader)
	{
		int[] version = reader.version;
		int numNodes = reader.ReadInt32();
		m_NodeArray = new BlendTreeNodeConstant[numNodes];
		for (int i = 0; i < numNodes; i++)
		{
			m_NodeArray[i] = new BlendTreeNodeConstant(reader);
		}
		if (version[0] < 4 || (version[0] == 4 && version[1] < 5))
		{
			m_BlendEventArrayConstant = new ValueArrayConstant(reader);
		}
	}
}
