using System;
using System.IO;
using SevenZip.Compression.LZMA;

namespace AssetStudio;

public static class SevenZipHelper
{
	public static MemoryStream StreamDecompress(MemoryStream inStream)
	{
		Decoder decoder = new Decoder();
		inStream.Seek(0L, SeekOrigin.Begin);
		MemoryStream newOutStream = new MemoryStream();
		byte[] properties = new byte[5];
		if (inStream.Read(properties, 0, 5) != 5)
		{
			throw new Exception("input .lzma is too short");
		}
		long outSize = 0L;
		for (int i = 0; i < 8; i++)
		{
			int v = inStream.ReadByte();
			if (v < 0)
			{
				throw new Exception("Can't Read 1");
			}
			outSize |= (long)((ulong)(byte)v << 8 * i);
		}
		decoder.SetDecoderProperties(properties);
		long compressedSize = inStream.Length - inStream.Position;
		decoder.Code(inStream, newOutStream, compressedSize, outSize, null);
		newOutStream.Position = 0L;
		return newOutStream;
	}

	public static void StreamDecompress(Stream compressedStream, Stream decompressedStream, long compressedSize, long decompressedSize)
	{
		long basePosition = compressedStream.Position;
		Decoder decoder = new Decoder();
		byte[] properties = new byte[5];
		if (compressedStream.Read(properties, 0, 5) != 5)
		{
			throw new Exception("input .lzma is too short");
		}
		decoder.SetDecoderProperties(properties);
		decoder.Code(compressedStream, decompressedStream, compressedSize - 5, decompressedSize, null);
		compressedStream.Position = basePosition + compressedSize;
	}
}
