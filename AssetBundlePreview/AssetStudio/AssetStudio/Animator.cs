namespace AssetStudio;

public sealed class Animator : Behaviour
{
	public PPtr<Avatar> m_Avatar;

	public PPtr<RuntimeAnimatorController> m_Controller;

	public bool m_HasTransformHierarchy = true;

	public Animator(ObjectReader reader)
		: base(reader)
	{
		m_Avatar = new PPtr<Avatar>(reader);
		m_Controller = new PPtr<RuntimeAnimatorController>(reader);
		reader.ReadInt32();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 5))
		{
			reader.ReadInt32();
		}
		reader.ReadBoolean();
		if (version[0] == 4 && version[1] >= 5)
		{
			reader.AlignStream();
		}
		if (version[0] >= 5)
		{
			reader.ReadBoolean();
			if (version[0] > 2021 || (version[0] == 2021 && version[1] >= 2))
			{
				reader.ReadBoolean();
			}
			reader.AlignStream();
		}
		if (version[0] < 4 || (version[0] == 4 && version[1] < 5))
		{
			reader.ReadBoolean();
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_HasTransformHierarchy = reader.ReadBoolean();
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 5))
		{
			reader.ReadBoolean();
		}
		if (version[0] >= 5 && version[0] < 2018)
		{
			reader.AlignStream();
		}
		if (version[0] >= 2018)
		{
			reader.ReadBoolean();
			reader.AlignStream();
		}
	}
}
