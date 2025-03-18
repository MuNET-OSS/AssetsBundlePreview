using System.Collections.Generic;

namespace AssetStudio;

public class ImportedAnimationKeyframedTrack
{
	public List<ImportedKeyframe<Vector3>> Scalings = new List<ImportedKeyframe<Vector3>>();

	public List<ImportedKeyframe<Vector3>> Rotations = new List<ImportedKeyframe<Vector3>>();

	public List<ImportedKeyframe<Vector3>> Translations = new List<ImportedKeyframe<Vector3>>();

	public ImportedBlendShape BlendShape;

	public string Path { get; set; }
}
