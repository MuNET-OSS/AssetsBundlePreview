using System.IO;

namespace AssetStudio;

public class SerializedStencilOp
{
	public SerializedShaderFloatValue pass;

	public SerializedShaderFloatValue fail;

	public SerializedShaderFloatValue zFail;

	public SerializedShaderFloatValue comp;

	public SerializedStencilOp(BinaryReader reader)
	{
		pass = new SerializedShaderFloatValue(reader);
		fail = new SerializedShaderFloatValue(reader);
		zFail = new SerializedShaderFloatValue(reader);
		comp = new SerializedShaderFloatValue(reader);
	}
}
