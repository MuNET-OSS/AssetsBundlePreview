using System.IO;

namespace AssetStudio;

public static class StreamExtensions
{
	private const int BufferSize = 81920;

	public static void CopyTo(this Stream source, Stream destination, long size)
	{
		byte[] buffer = new byte[81920];
		for (long left = size; left > 0; left -= 81920)
		{
			int toRead = (int)((81920 < left) ? 81920 : left);
			int read = source.Read(buffer, 0, toRead);
			destination.Write(buffer, 0, read);
			if (read != toRead)
			{
				break;
			}
		}
	}
}
