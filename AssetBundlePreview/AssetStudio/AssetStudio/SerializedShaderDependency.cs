using System.IO;

namespace AssetStudio;

public class SerializedShaderDependency
{
	public string from;

	public string to;

	public SerializedShaderDependency(BinaryReader reader)
	{
		from = reader.ReadAlignedString();
		to = reader.ReadAlignedString();
	}
}
