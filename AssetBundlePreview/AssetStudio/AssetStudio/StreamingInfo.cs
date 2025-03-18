namespace AssetStudio;

public class StreamingInfo
{
	public long offset;

	public uint size;

	public string path;

	public StreamingInfo(ObjectReader reader)
	{
		if (reader.version[0] >= 2020)
		{
			offset = reader.ReadInt64();
		}
		else
		{
			offset = reader.ReadUInt32();
		}
		size = reader.ReadUInt32();
		path = reader.ReadAlignedString();
	}
}
