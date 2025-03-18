using System;

namespace AssetStudio;

public static class MeshHelper
{
	public enum VertexChannelFormat
	{
		Float,
		Float16,
		Color,
		Byte,
		UInt32
	}

	public enum VertexFormat2017
	{
		Float,
		Float16,
		Color,
		UNorm8,
		SNorm8,
		UNorm16,
		SNorm16,
		UInt8,
		SInt8,
		UInt16,
		SInt16,
		UInt32,
		SInt32
	}

	public enum VertexFormat
	{
		Float,
		Float16,
		UNorm8,
		SNorm8,
		UNorm16,
		SNorm16,
		UInt8,
		SInt8,
		UInt16,
		SInt16,
		UInt32,
		SInt32
	}

	public static VertexFormat ToVertexFormat(int format, int[] version)
	{
		if (version[0] < 2017)
		{
			return (VertexChannelFormat)format switch
			{
				VertexChannelFormat.Float => VertexFormat.Float, 
				VertexChannelFormat.Float16 => VertexFormat.Float16, 
				VertexChannelFormat.Color => VertexFormat.UNorm8, 
				VertexChannelFormat.Byte => VertexFormat.UInt8, 
				VertexChannelFormat.UInt32 => VertexFormat.UInt32, 
				_ => throw new ArgumentOutOfRangeException("format", format, null), 
			};
		}
		if (version[0] < 2019)
		{
			switch ((VertexFormat2017)format)
			{
			case VertexFormat2017.Float:
				return VertexFormat.Float;
			case VertexFormat2017.Float16:
				return VertexFormat.Float16;
			case VertexFormat2017.Color:
			case VertexFormat2017.UNorm8:
				return VertexFormat.UNorm8;
			case VertexFormat2017.SNorm8:
				return VertexFormat.SNorm8;
			case VertexFormat2017.UNorm16:
				return VertexFormat.UNorm16;
			case VertexFormat2017.SNorm16:
				return VertexFormat.SNorm16;
			case VertexFormat2017.UInt8:
				return VertexFormat.UInt8;
			case VertexFormat2017.SInt8:
				return VertexFormat.SInt8;
			case VertexFormat2017.UInt16:
				return VertexFormat.UInt16;
			case VertexFormat2017.SInt16:
				return VertexFormat.SInt16;
			case VertexFormat2017.UInt32:
				return VertexFormat.UInt32;
			case VertexFormat2017.SInt32:
				return VertexFormat.SInt32;
			default:
				throw new ArgumentOutOfRangeException("format", format, null);
			}
		}
		return (VertexFormat)format;
	}

	public static uint GetFormatSize(VertexFormat format)
	{
		switch (format)
		{
		case VertexFormat.Float:
		case VertexFormat.UInt32:
		case VertexFormat.SInt32:
			return 4u;
		case VertexFormat.Float16:
		case VertexFormat.UNorm16:
		case VertexFormat.SNorm16:
		case VertexFormat.UInt16:
		case VertexFormat.SInt16:
			return 2u;
		case VertexFormat.UNorm8:
		case VertexFormat.SNorm8:
		case VertexFormat.UInt8:
		case VertexFormat.SInt8:
			return 1u;
		default:
			throw new ArgumentOutOfRangeException("format", format, null);
		}
	}

	public static bool IsIntFormat(VertexFormat format)
	{
		return format >= VertexFormat.UInt8;
	}

	public static float[] BytesToFloatArray(byte[] inputBytes, VertexFormat format)
	{
		uint size = GetFormatSize(format);
		long len = inputBytes.Length / size;
		float[] result = new float[len];
		for (int i = 0; i < len; i++)
		{
			switch (format)
			{
			case VertexFormat.Float:
				result[i] = BitConverter.ToSingle(inputBytes, i * 4);
				break;
			case VertexFormat.Float16:
				result[i] = Half.ToHalf(inputBytes, i * 2);
				break;
			case VertexFormat.UNorm8:
				result[i] = (float)(int)inputBytes[i] / 255f;
				break;
			case VertexFormat.SNorm8:
				result[i] = Math.Max((float)(sbyte)inputBytes[i] / 127f, -1f);
				break;
			case VertexFormat.UNorm16:
				result[i] = (float)(int)BitConverter.ToUInt16(inputBytes, i * 2) / 65535f;
				break;
			case VertexFormat.SNorm16:
				result[i] = Math.Max((float)BitConverter.ToInt16(inputBytes, i * 2) / 32767f, -1f);
				break;
			}
		}
		return result;
	}

	public static int[] BytesToIntArray(byte[] inputBytes, VertexFormat format)
	{
		uint size = GetFormatSize(format);
		long len = inputBytes.Length / size;
		int[] result = new int[len];
		for (int i = 0; i < len; i++)
		{
			switch (format)
			{
			case VertexFormat.UInt8:
			case VertexFormat.SInt8:
				result[i] = inputBytes[i];
				break;
			case VertexFormat.UInt16:
			case VertexFormat.SInt16:
				result[i] = BitConverter.ToInt16(inputBytes, i * 2);
				break;
			case VertexFormat.UInt32:
			case VertexFormat.SInt32:
				result[i] = BitConverter.ToInt32(inputBytes, i * 4);
				break;
			}
		}
		return result;
	}
}
