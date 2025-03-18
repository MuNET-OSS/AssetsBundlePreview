namespace AssetStudio;

public abstract class Renderer : Component
{
	public PPtr<Material>[] m_Materials;

	public StaticBatchInfo m_StaticBatchInfo;

	public uint[] m_SubsetIndices;

	protected Renderer(ObjectReader reader)
		: base(reader)
	{
		if (version[0] < 5)
		{
			reader.ReadBoolean();
			reader.ReadBoolean();
			reader.ReadBoolean();
			reader.ReadByte();
		}
		else
		{
			if (version[0] > 5 || (version[0] == 5 && version[1] >= 4))
			{
				reader.ReadBoolean();
				reader.ReadByte();
				reader.ReadByte();
				if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 2))
				{
					reader.ReadByte();
				}
				if (version[0] >= 2021)
				{
					reader.ReadByte();
				}
				reader.ReadByte();
				reader.ReadByte();
				reader.ReadByte();
				if (version[0] > 2019 || (version[0] == 2019 && version[1] >= 3))
				{
					reader.ReadByte();
				}
				if (version[0] >= 2020)
				{
					reader.ReadByte();
				}
				reader.AlignStream();
			}
			else
			{
				reader.ReadBoolean();
				reader.AlignStream();
				reader.ReadByte();
				reader.ReadBoolean();
				reader.AlignStream();
			}
			if (version[0] >= 2018)
			{
				reader.ReadUInt32();
			}
			if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 3))
			{
				reader.ReadInt32();
			}
			reader.ReadUInt16();
			reader.ReadUInt16();
		}
		if (version[0] >= 3)
		{
			reader.ReadVector4();
		}
		if (version[0] >= 5)
		{
			reader.ReadVector4();
		}
		int m_MaterialsSize = reader.ReadInt32();
		m_Materials = new PPtr<Material>[m_MaterialsSize];
		for (int i = 0; i < m_MaterialsSize; i++)
		{
			m_Materials[i] = new PPtr<Material>(reader);
		}
		if (version[0] < 3)
		{
			reader.ReadVector4();
		}
		else
		{
			if (version[0] > 5 || (version[0] == 5 && version[1] >= 5))
			{
				m_StaticBatchInfo = new StaticBatchInfo(reader);
			}
			else
			{
				m_SubsetIndices = reader.ReadUInt32Array();
			}
			new PPtr<Transform>(reader);
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 4))
		{
			new PPtr<Transform>(reader);
			new PPtr<GameObject>(reader);
		}
		else if (version[0] > 3 || (version[0] == 3 && version[1] >= 5))
		{
			reader.ReadBoolean();
			reader.AlignStream();
			if (version[0] >= 5)
			{
				reader.ReadInt32();
			}
			new PPtr<Transform>(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			if (version[0] == 4 && version[1] == 3)
			{
				reader.ReadInt16();
			}
			else
			{
				reader.ReadUInt32();
			}
			reader.ReadInt16();
			reader.AlignStream();
		}
	}
}
