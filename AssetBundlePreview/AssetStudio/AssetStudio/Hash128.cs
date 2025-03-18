using System.IO;

namespace AssetStudio;

public class Hash128
{
	public byte[] bytes;

	public Hash128(BinaryReader reader)
	{
		bytes = reader.ReadBytes(16);
	}
}
