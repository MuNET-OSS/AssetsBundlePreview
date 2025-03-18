namespace AssetStudio;

public class AssetInfo
{
	public int preloadIndex;

	public int preloadSize;

	public PPtr<Object> asset;

	public AssetInfo(ObjectReader reader)
	{
		preloadIndex = reader.ReadInt32();
		preloadSize = reader.ReadInt32();
		asset = new PPtr<Object>(reader);
	}
}
