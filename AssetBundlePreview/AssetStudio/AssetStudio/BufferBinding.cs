namespace AssetStudio;

public class BufferBinding
{
	public int m_NameIndex;

	public int m_Index;

	public int m_ArraySize;

	public BufferBinding(ObjectReader reader)
	{
		int[] version = reader.version;
		m_NameIndex = reader.ReadInt32();
		m_Index = reader.ReadInt32();
		if (version[0] >= 2020)
		{
			m_ArraySize = reader.ReadInt32();
		}
	}
}
