namespace AssetStudio;

public abstract class EditorExtension : Object
{
	protected EditorExtension(ObjectReader reader)
		: base(reader)
	{
		if (platform == BuildTarget.NoTarget)
		{
			new PPtr<EditorExtension>(reader);
			new PPtr<Object>(reader);
		}
	}
}
