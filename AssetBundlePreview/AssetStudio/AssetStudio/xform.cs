namespace AssetStudio;

public class xform
{
	public Vector3 t;

	public Quaternion q;

	public Vector3 s;

	public xform(ObjectReader reader)
	{
		int[] version = reader.version;
		t = ((version[0] > 5 || (version[0] == 5 && version[1] >= 4)) ? reader.ReadVector3() : ((Vector3)reader.ReadVector4()));
		q = reader.ReadQuaternion();
		s = ((version[0] > 5 || (version[0] == 5 && version[1] >= 4)) ? reader.ReadVector3() : ((Vector3)reader.ReadVector4()));
	}
}
