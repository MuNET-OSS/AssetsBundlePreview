using System.Collections.Generic;

namespace AssetStudio;

public sealed class AssetBundle : NamedObject
{
	public PPtr<Object>[] m_PreloadTable;

	public KeyValuePair<string, AssetInfo>[] m_Container;

	public AssetBundle(ObjectReader reader)
		: base(reader)
	{
		int m_PreloadTableSize = reader.ReadInt32();
		m_PreloadTable = new PPtr<Object>[m_PreloadTableSize];
		for (int j = 0; j < m_PreloadTableSize; j++)
		{
			m_PreloadTable[j] = new PPtr<Object>(reader);
		}
		int m_ContainerSize = reader.ReadInt32();
		m_Container = new KeyValuePair<string, AssetInfo>[m_ContainerSize];
		for (int i = 0; i < m_ContainerSize; i++)
		{
			m_Container[i] = new KeyValuePair<string, AssetInfo>(reader.ReadAlignedString(), new AssetInfo(reader));
		}
	}
}
