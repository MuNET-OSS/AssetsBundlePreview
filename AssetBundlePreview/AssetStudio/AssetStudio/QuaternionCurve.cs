namespace AssetStudio;

public class QuaternionCurve
{
	public AnimationCurve<Quaternion> curve;

	public string path;

	public QuaternionCurve(ObjectReader reader)
	{
		curve = new AnimationCurve<Quaternion>(reader, reader.ReadQuaternion);
		path = reader.ReadAlignedString();
	}
}
