using System.Collections.Generic;

namespace AssetStudio;

public class ImportedMorphChannel
{
	public string Name { get; set; }

	public List<ImportedMorphKeyframe> KeyframeList { get; set; }
}
