using System;
using System.Runtime.InteropServices;

namespace AssetStudio;

[ComVisible(false)]
internal static class HalfHelper
{
	private static uint[] mantissaTable = GenerateMantissaTable();

	private static uint[] exponentTable = GenerateExponentTable();

	private static ushort[] offsetTable = GenerateOffsetTable();

	private static ushort[] baseTable = GenerateBaseTable();

	private static sbyte[] shiftTable = GenerateShiftTable();

	private static uint ConvertMantissa(int i)
	{
		uint j = (uint)(i << 13);
		uint e = 0u;
		while ((j & 0x800000) == 0)
		{
			e -= 8388608;
			j <<= 1;
		}
		j &= 0xFF7FFFFFu;
		e += 947912704;
		return j | e;
	}

	private static uint[] GenerateMantissaTable()
	{
		uint[] mantissaTable = new uint[2048];
		mantissaTable[0] = 0u;
		for (int j = 1; j < 1024; j++)
		{
			mantissaTable[j] = ConvertMantissa(j);
		}
		for (int i = 1024; i < 2048; i++)
		{
			mantissaTable[i] = (uint)(939524096 + (i - 1024 << 13));
		}
		return mantissaTable;
	}

	private static uint[] GenerateExponentTable()
	{
		uint[] exponentTable = new uint[64];
		exponentTable[0] = 0u;
		for (int j = 1; j < 31; j++)
		{
			exponentTable[j] = (uint)(j << 23);
		}
		exponentTable[31] = 1199570944u;
		exponentTable[32] = 2147483648u;
		for (int i = 33; i < 63; i++)
		{
			exponentTable[i] = (uint)(2147483648u + (i - 32 << 23));
		}
		exponentTable[63] = 3347054592u;
		return exponentTable;
	}

	private static ushort[] GenerateOffsetTable()
	{
		ushort[] offsetTable = new ushort[64];
		offsetTable[0] = 0;
		for (int j = 1; j < 32; j++)
		{
			offsetTable[j] = 1024;
		}
		offsetTable[32] = 0;
		for (int i = 33; i < 64; i++)
		{
			offsetTable[i] = 1024;
		}
		return offsetTable;
	}

	private static ushort[] GenerateBaseTable()
	{
		ushort[] baseTable = new ushort[512];
		for (int i = 0; i < 256; i++)
		{
			sbyte e = (sbyte)(127 - i);
			if (e > 24)
			{
				baseTable[i | 0] = 0;
				baseTable[i | 0x100] = 32768;
			}
			else if (e > 14)
			{
				baseTable[i | 0] = (ushort)(1024 >> 18 + e);
				baseTable[i | 0x100] = (ushort)((uint)(1024 >> 18 + e) | 0x8000u);
			}
			else if (e >= -15)
			{
				baseTable[i | 0] = (ushort)(15 - e << 10);
				baseTable[i | 0x100] = (ushort)((uint)(15 - e << 10) | 0x8000u);
			}
			else if (e > sbyte.MinValue)
			{
				baseTable[i | 0] = 31744;
				baseTable[i | 0x100] = 64512;
			}
			else
			{
				baseTable[i | 0] = 31744;
				baseTable[i | 0x100] = 64512;
			}
		}
		return baseTable;
	}

	private static sbyte[] GenerateShiftTable()
	{
		sbyte[] shiftTable = new sbyte[512];
		for (int i = 0; i < 256; i++)
		{
			sbyte e = (sbyte)(127 - i);
			if (e > 24)
			{
				shiftTable[i | 0] = 24;
				shiftTable[i | 0x100] = 24;
			}
			else if (e > 14)
			{
				shiftTable[i | 0] = (sbyte)(e - 1);
				shiftTable[i | 0x100] = (sbyte)(e - 1);
			}
			else if (e >= -15)
			{
				shiftTable[i | 0] = 13;
				shiftTable[i | 0x100] = 13;
			}
			else if (e > sbyte.MinValue)
			{
				shiftTable[i | 0] = 24;
				shiftTable[i | 0x100] = 24;
			}
			else
			{
				shiftTable[i | 0] = 13;
				shiftTable[i | 0x100] = 13;
			}
		}
		return shiftTable;
	}

	public static float HalfToSingle(Half half)
	{
		return BitConverter.ToSingle(BitConverter.GetBytes(mantissaTable[offsetTable[half.value >> 10] + (half.value & 0x3FF)] + exponentTable[half.value >> 10]), 0);
	}

	public static Half SingleToHalf(float single)
	{
		uint value = BitConverter.ToUInt32(BitConverter.GetBytes(single), 0);
		return Half.ToHalf((ushort)(baseTable[(value >> 23) & 0x1FF] + ((value & 0x7FFFFF) >> (int)shiftTable[value >> 23])));
	}

	public static Half Negate(Half half)
	{
		return Half.ToHalf((ushort)(half.value ^ 0x8000u));
	}

	public static Half Abs(Half half)
	{
		return Half.ToHalf((ushort)(half.value & 0x7FFFu));
	}

	public static bool IsNaN(Half half)
	{
		return (half.value & 0x7FFF) > 31744;
	}

	public static bool IsInfinity(Half half)
	{
		return (half.value & 0x7FFF) == 31744;
	}

	public static bool IsPositiveInfinity(Half half)
	{
		return half.value == 31744;
	}

	public static bool IsNegativeInfinity(Half half)
	{
		return half.value == 64512;
	}
}
