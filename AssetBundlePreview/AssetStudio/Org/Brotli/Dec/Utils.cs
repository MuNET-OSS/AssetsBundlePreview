using System;

namespace Org.Brotli.Dec;

internal sealed class Utils
{
	private static readonly byte[] ByteZeroes = new byte[1024];

	private static readonly int[] IntZeroes = new int[1024];

	internal static void FillWithZeroes(byte[] dest, int offset, int length)
	{
		int step;
		for (int cursor = 0; cursor < length; cursor += step)
		{
			step = Math.Min(cursor + 1024, length) - cursor;
			Array.Copy(ByteZeroes, 0, dest, offset + cursor, step);
		}
	}

	internal static void FillWithZeroes(int[] dest, int offset, int length)
	{
		int step;
		for (int cursor = 0; cursor < length; cursor += step)
		{
			step = Math.Min(cursor + 1024, length) - cursor;
			Array.Copy(IntZeroes, 0, dest, offset + cursor, step);
		}
	}
}
