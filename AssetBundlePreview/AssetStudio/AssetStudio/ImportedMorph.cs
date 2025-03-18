using System.Collections.Generic;

namespace AssetStudio;

public class ImportedMorph
{
	public string Path { get; set; }

	public List<ImportedMorphChannel> Channels { get; set; }
}
