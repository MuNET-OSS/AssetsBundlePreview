using System.IO;
using System.Text;

namespace AssetStudio;

public static class BinaryWriterExtensions
{
	public static void AlignStream(this BinaryWriter writer, int alignment)
	{
		long mod = writer.BaseStream.Position % alignment;
		if (mod != 0L)
		{
			writer.Write(new byte[alignment - mod]);
		}
	}

	public static void WriteAlignedString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		writer.Write(bytes);
		writer.AlignStream(4);
	}
}
