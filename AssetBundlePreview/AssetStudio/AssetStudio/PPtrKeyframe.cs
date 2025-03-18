namespace AssetStudio;

public class PPtrKeyframe
{
	public float time;

	public PPtr<Object> value;

	public PPtrKeyframe(ObjectReader reader)
	{
		time = reader.ReadSingle();
		value = new PPtr<Object>(reader);
	}
}
