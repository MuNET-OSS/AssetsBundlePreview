namespace AssetStudio;

public class GenericBinding
{
	public uint path;

	public uint attribute;

	public PPtr<Object> script;

	public ClassIDType typeID;

	public byte customType;

	public byte isPPtrCurve;

	public byte isIntCurve;

	public GenericBinding()
	{
	}

	public GenericBinding(ObjectReader reader)
	{
		int[] version = reader.version;
		path = reader.ReadUInt32();
		attribute = reader.ReadUInt32();
		script = new PPtr<Object>(reader);
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 6))
		{
			typeID = (ClassIDType)reader.ReadInt32();
		}
		else
		{
			typeID = (ClassIDType)reader.ReadUInt16();
		}
		customType = reader.ReadByte();
		isPPtrCurve = reader.ReadByte();
		if (version[0] > 2022 || (version[0] == 2022 && version[1] >= 1))
		{
			isIntCurve = reader.ReadByte();
		}
		reader.AlignStream();
	}
}
