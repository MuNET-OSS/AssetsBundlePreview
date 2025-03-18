using System.IO;

namespace AssetStudio;

public class ImportedTexture
{
	public string Name { get; set; }

	public byte[] Data { get; set; }

	public ImportedTexture(MemoryStream stream, string name)
	{
		Name = name;
		Data = stream.ToArray();
	}
}
