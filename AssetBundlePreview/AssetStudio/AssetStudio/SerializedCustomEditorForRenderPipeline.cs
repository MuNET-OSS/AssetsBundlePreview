using System.IO;

namespace AssetStudio;

public class SerializedCustomEditorForRenderPipeline
{
	public string customEditorName;

	public string renderPipelineType;

	public SerializedCustomEditorForRenderPipeline(BinaryReader reader)
	{
		customEditorName = reader.ReadAlignedString();
		renderPipelineType = reader.ReadAlignedString();
	}
}
