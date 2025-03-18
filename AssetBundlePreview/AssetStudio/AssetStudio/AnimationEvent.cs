namespace AssetStudio;

public class AnimationEvent
{
	public float time;

	public string functionName;

	public string data;

	public PPtr<Object> objectReferenceParameter;

	public float floatParameter;

	public int intParameter;

	public int messageOptions;

	public AnimationEvent(ObjectReader reader)
	{
		int[] version = reader.version;
		time = reader.ReadSingle();
		functionName = reader.ReadAlignedString();
		data = reader.ReadAlignedString();
		objectReferenceParameter = new PPtr<Object>(reader);
		floatParameter = reader.ReadSingle();
		if (version[0] >= 3)
		{
			intParameter = reader.ReadInt32();
		}
		messageOptions = reader.ReadInt32();
	}
}
