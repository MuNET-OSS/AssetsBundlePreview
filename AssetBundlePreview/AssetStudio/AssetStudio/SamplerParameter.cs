using System.IO;

namespace AssetStudio;

public class SamplerParameter
{
	public uint sampler;

	public int bindPoint;

	public SamplerParameter(BinaryReader reader)
	{
		sampler = reader.ReadUInt32();
		bindPoint = reader.ReadInt32();
	}
}
