namespace AssetStudio;

public class StreamInfo
{
	public uint channelMask;

	public uint offset;

	public uint stride;

	public uint align;

	public byte dividerOp;

	public ushort frequency;

	public StreamInfo()
	{
	}

	public StreamInfo(ObjectReader reader)
	{
		int[] version = reader.version;
		channelMask = reader.ReadUInt32();
		offset = reader.ReadUInt32();
		if (version[0] < 4)
		{
			stride = reader.ReadUInt32();
			align = reader.ReadUInt32();
		}
		else
		{
			stride = reader.ReadByte();
			dividerOp = reader.ReadByte();
			frequency = reader.ReadUInt16();
		}
	}
}
