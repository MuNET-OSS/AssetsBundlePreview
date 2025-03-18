using System;
using System.IO;

namespace Org.Brotli.Dec;

internal sealed class BitReader
{
	private const int Capacity = 1024;

	private const int Slack = 16;

	private const int IntBufferSize = 1040;

	private const int ByteReadSize = 4096;

	private const int ByteBufferSize = 4160;

	private readonly byte[] byteBuffer = new byte[4160];

	private readonly int[] intBuffer = new int[1040];

	private readonly IntReader intReader = new IntReader();

	private Stream input;

	private bool endOfStreamReached;

	internal long accumulator;

	internal int bitOffset;

	private int intOffset;

	private int tailBytes;

	internal static void ReadMoreInput(BitReader br)
	{
		if (br.intOffset <= 1015)
		{
			return;
		}
		if (br.endOfStreamReached)
		{
			if (IntAvailable(br) >= -2)
			{
				return;
			}
			throw new BrotliRuntimeException("No more input");
		}
		int readOffset = br.intOffset << 2;
		int bytesRead = 4096 - readOffset;
		Array.Copy(br.byteBuffer, readOffset, br.byteBuffer, 0, bytesRead);
		br.intOffset = 0;
		try
		{
			int len;
			for (; bytesRead < 4096; bytesRead += len)
			{
				len = br.input.Read(br.byteBuffer, bytesRead, 4096 - bytesRead);
				if (len <= 0)
				{
					br.endOfStreamReached = true;
					br.tailBytes = bytesRead;
					bytesRead += 3;
					break;
				}
			}
		}
		catch (IOException e)
		{
			throw new BrotliRuntimeException("Failed to read input", e);
		}
		IntReader.Convert(br.intReader, bytesRead >> 2);
	}

	internal static void CheckHealth(BitReader br, bool endOfStream)
	{
		if (br.endOfStreamReached)
		{
			int byteOffset = (br.intOffset << 2) + (br.bitOffset + 7 >> 3) - 8;
			if (byteOffset > br.tailBytes)
			{
				throw new BrotliRuntimeException("Read after end");
			}
			if (endOfStream && byteOffset != br.tailBytes)
			{
				throw new BrotliRuntimeException("Unused bytes after end");
			}
		}
	}

	internal static void FillBitWindow(BitReader br)
	{
		if (br.bitOffset >= 32)
		{
			br.accumulator = ((long)br.intBuffer[br.intOffset++] << 32) | (br.accumulator >>> 32);
			br.bitOffset -= 32;
		}
	}

	internal static int ReadBits(BitReader br, int n)
	{
		FillBitWindow(br);
		int result = (int)(br.accumulator >>> br.bitOffset) & ((1 << n) - 1);
		br.bitOffset += n;
		return result;
	}

	internal static void Init(BitReader br, Stream input)
	{
		if (br.input != null)
		{
			throw new InvalidOperationException("Bit reader already has associated input stream");
		}
		IntReader.Init(br.intReader, br.byteBuffer, br.intBuffer);
		br.input = input;
		br.accumulator = 0L;
		br.bitOffset = 64;
		br.intOffset = 1024;
		br.endOfStreamReached = false;
		Prepare(br);
	}

	private static void Prepare(BitReader br)
	{
		ReadMoreInput(br);
		CheckHealth(br, endOfStream: false);
		FillBitWindow(br);
		FillBitWindow(br);
	}

	internal static void Reload(BitReader br)
	{
		if (br.bitOffset == 64)
		{
			Prepare(br);
		}
	}

	internal static void Close(BitReader br)
	{
		Stream @is = br.input;
		br.input = null;
		@is?.Close();
	}

	internal static void JumpToByteBoundary(BitReader br)
	{
		int padding = (64 - br.bitOffset) & 7;
		if (padding != 0 && ReadBits(br, padding) != 0)
		{
			throw new BrotliRuntimeException("Corrupted padding bits");
		}
	}

	internal static int IntAvailable(BitReader br)
	{
		int limit = 1024;
		if (br.endOfStreamReached)
		{
			limit = br.tailBytes + 3 >> 2;
		}
		return limit - br.intOffset;
	}

	internal static void CopyBytes(BitReader br, byte[] data, int offset, int length)
	{
		if (((uint)br.bitOffset & 7u) != 0)
		{
			throw new BrotliRuntimeException("Unaligned copyBytes");
		}
		while (br.bitOffset != 64 && length != 0)
		{
			data[offset++] = (byte)((ulong)br.accumulator >> br.bitOffset);
			br.bitOffset += 8;
			length--;
		}
		if (length == 0)
		{
			return;
		}
		int copyInts = Math.Min(IntAvailable(br), length >> 2);
		if (copyInts > 0)
		{
			int readOffset = br.intOffset << 2;
			Array.Copy(br.byteBuffer, readOffset, data, offset, copyInts << 2);
			offset += copyInts << 2;
			length -= copyInts << 2;
			br.intOffset += copyInts;
		}
		if (length == 0)
		{
			return;
		}
		if (IntAvailable(br) > 0)
		{
			FillBitWindow(br);
			while (length != 0)
			{
				data[offset++] = (byte)((ulong)br.accumulator >> br.bitOffset);
				br.bitOffset += 8;
				length--;
			}
			CheckHealth(br, endOfStream: false);
			return;
		}
		try
		{
			while (length > 0)
			{
				int len = br.input.Read(data, offset, length);
				if (len == -1)
				{
					throw new BrotliRuntimeException("Unexpected end of input");
				}
				offset += len;
				length -= len;
			}
		}
		catch (IOException e)
		{
			throw new BrotliRuntimeException("Failed to read input", e);
		}
	}
}
