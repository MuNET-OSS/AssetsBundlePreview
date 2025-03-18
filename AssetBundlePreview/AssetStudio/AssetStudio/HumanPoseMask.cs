namespace AssetStudio;

public class HumanPoseMask
{
	public uint word0;

	public uint word1;

	public uint word2;

	public HumanPoseMask(ObjectReader reader)
	{
		int[] version = reader.version;
		word0 = reader.ReadUInt32();
		word1 = reader.ReadUInt32();
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 2))
		{
			word2 = reader.ReadUInt32();
		}
	}
}
