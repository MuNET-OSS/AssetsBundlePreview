namespace AssetStudio;

public class AnimationClipBindingConstant
{
	public GenericBinding[] genericBindings;

	public PPtr<Object>[] pptrCurveMapping;

	public AnimationClipBindingConstant()
	{
	}

	public AnimationClipBindingConstant(ObjectReader reader)
	{
		int numBindings = reader.ReadInt32();
		genericBindings = new GenericBinding[numBindings];
		for (int j = 0; j < numBindings; j++)
		{
			genericBindings[j] = new GenericBinding(reader);
		}
		int numMappings = reader.ReadInt32();
		pptrCurveMapping = new PPtr<Object>[numMappings];
		for (int i = 0; i < numMappings; i++)
		{
			pptrCurveMapping[i] = new PPtr<Object>(reader);
		}
	}

	public GenericBinding FindBinding(int index)
	{
		int curves = 0;
		GenericBinding[] array = genericBindings;
		foreach (GenericBinding b in array)
		{
			if (b.typeID == ClassIDType.Transform)
			{
				switch (b.attribute)
				{
				case 1u:
				case 3u:
				case 4u:
					curves += 3;
					break;
				case 2u:
					curves += 4;
					break;
				default:
					curves++;
					break;
				}
			}
			else
			{
				curves++;
			}
			if (curves > index)
			{
				return b;
			}
		}
		return null;
	}
}
