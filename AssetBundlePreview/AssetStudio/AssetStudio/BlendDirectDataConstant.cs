namespace AssetStudio;

public class BlendDirectDataConstant
{
	public uint[] m_ChildBlendEventIDArray;

	public bool m_NormalizedBlendValues;

	public BlendDirectDataConstant(ObjectReader reader)
	{
		m_ChildBlendEventIDArray = reader.ReadUInt32Array();
		m_NormalizedBlendValues = reader.ReadBoolean();
		reader.AlignStream();
	}
}
