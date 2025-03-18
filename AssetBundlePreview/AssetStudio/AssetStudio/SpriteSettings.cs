using System.IO;

namespace AssetStudio;

public class SpriteSettings
{
	public uint settingsRaw;

	public uint packed;

	public SpritePackingMode packingMode;

	public SpritePackingRotation packingRotation;

	public SpriteMeshType meshType;

	public SpriteSettings(BinaryReader reader)
	{
		settingsRaw = reader.ReadUInt32();
		packed = settingsRaw & 1u;
		packingMode = (SpritePackingMode)((int)(settingsRaw >> 1) & 1);
		packingRotation = (SpritePackingRotation)((int)(settingsRaw >> 2) & 0xF);
		meshType = (SpriteMeshType)((int)(settingsRaw >> 6) & 1);
	}
}
