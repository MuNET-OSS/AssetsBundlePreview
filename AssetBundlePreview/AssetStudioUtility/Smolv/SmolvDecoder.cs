using System;
using System.IO;
using System.Text;

namespace Smolv;

public static class SmolvDecoder
{
	public const uint SpirVHeaderMagic = 119734787u;

	public const uint SmolHeaderMagic = 1397575500u;

	private const int HeaderSize = 24;

	public static int GetDecodedBufferSize(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (!CheckSmolHeader(data))
		{
			return 0;
		}
		return BitConverter.ToInt32(data, 20);
	}

	public static int GetDecodedBufferSize(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanSeek)
		{
			throw new ArgumentException("stream");
		}
		if (stream.Position + 24 > stream.Length)
		{
			return 0;
		}
		long initPosition = stream.Position;
		stream.Position += 20L;
		int result = stream.ReadByte() | (stream.ReadByte() << 8) | (stream.ReadByte() << 16) | (stream.ReadByte() << 24);
		stream.Position = initPosition;
		return result;
	}

	public static byte[] Decode(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		int bufferSize = GetDecodedBufferSize(data);
		if (bufferSize == 0)
		{
			return null;
		}
		byte[] output = new byte[bufferSize];
		if (Decode(data, output))
		{
			return output;
		}
		return null;
	}

	public static bool Decode(byte[] data, byte[] output)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (GetDecodedBufferSize(data) > output.Length)
		{
			return false;
		}
		using MemoryStream outputStream = new MemoryStream(output);
		return Decode(data, outputStream);
	}

	public static bool Decode(byte[] data, Stream outputStream)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		using MemoryStream inputStream = new MemoryStream(data);
		return Decode(inputStream, data.Length, outputStream);
	}

	public static bool Decode(Stream inputStream, int inputSize, Stream outputStream)
	{
		if (inputStream == null)
		{
			throw new ArgumentNullException("inputStream");
		}
		if (outputStream == null)
		{
			throw new ArgumentNullException("outputStream");
		}
		if (inputStream.Length < 24)
		{
			return false;
		}
		using BinaryReader input = new BinaryReader(inputStream, Encoding.UTF8, leaveOpen: true);
		using BinaryWriter output = new BinaryWriter(outputStream, Encoding.UTF8, leaveOpen: true);
		long inputEndPosition = input.BaseStream.Position + inputSize;
		long outputStartPosition = output.BaseStream.Position;
		output.Write(119734787u);
		input.BaseStream.Position += 4L;
		uint version = input.ReadUInt32();
		output.Write(version);
		uint generator = input.ReadUInt32();
		output.Write(generator);
		int bound = input.ReadInt32();
		output.Write(bound);
		uint schema = input.ReadUInt32();
		output.Write(schema);
		int decodedSize = input.ReadInt32();
		int prevResult = 0;
		int prevDecorate = 0;
		while (input.BaseStream.Position < inputEndPosition)
		{
			if (!ReadLengthOp(input, out var instrLen, out var op))
			{
				return false;
			}
			bool wasSwizzle = op == SpvOp.VectorShuffleCompact;
			if (wasSwizzle)
			{
				op = SpvOp.VectorShuffle;
			}
			output.Write((instrLen << 16) | (uint)op);
			uint ioffs = 1u;
			if (op.OpHasType())
			{
				if (!ReadVarint(input, out var value5))
				{
					return false;
				}
				output.Write(value5);
				ioffs++;
			}
			if (op.OpHasResult())
			{
				if (!ReadVarint(input, out var value4))
				{
					return false;
				}
				int zds2 = prevResult + ZigDecode(value4);
				output.Write(zds2);
				prevResult = zds2;
				ioffs++;
			}
			if (op == SpvOp.Decorate || op == SpvOp.MemberDecorate)
			{
				if (!ReadVarint(input, out var value3))
				{
					return false;
				}
				int zds = prevDecorate + (int)value3;
				output.Write(zds);
				prevDecorate = zds;
				ioffs++;
			}
			int relativeCount = op.OpDeltaFromResult();
			bool inverted = false;
			if (relativeCount < 0)
			{
				inverted = true;
				relativeCount = -relativeCount;
			}
			int i = 0;
			while (i < relativeCount && ioffs < instrLen)
			{
				if (!ReadVarint(input, out var value2))
				{
					return false;
				}
				int zd = (inverted ? ZigDecode(value2) : ((int)value2));
				output.Write(prevResult - zd);
				i++;
				ioffs++;
			}
			if (wasSwizzle && instrLen <= 9)
			{
				uint swizzle = input.ReadByte();
				if (instrLen > 5)
				{
					output.Write(swizzle >> 6);
				}
				if (instrLen > 6)
				{
					output.Write((swizzle >> 4) & 3u);
				}
				if (instrLen > 7)
				{
					output.Write((swizzle >> 2) & 3u);
				}
				if (instrLen > 8)
				{
					output.Write(swizzle & 3u);
				}
				continue;
			}
			if (op.OpVarRest())
			{
				for (; ioffs < instrLen; ioffs++)
				{
					if (!ReadVarint(input, out var value))
					{
						return false;
					}
					output.Write(value);
				}
				continue;
			}
			for (; ioffs < instrLen; ioffs++)
			{
				if (input.BaseStream.Position + 4 > input.BaseStream.Length)
				{
					return false;
				}
				uint val = input.ReadUInt32();
				output.Write(val);
			}
		}
		if (output.BaseStream.Position != outputStartPosition + decodedSize)
		{
			return false;
		}
		return true;
	}

	private static bool CheckSmolHeader(byte[] data)
	{
		if (!CheckGenericHeader(data, 1397575500u))
		{
			return false;
		}
		return true;
	}

	private static bool CheckGenericHeader(byte[] data, uint expectedMagic)
	{
		if (data == null)
		{
			return false;
		}
		if (data.Length < 24)
		{
			return false;
		}
		if (BitConverter.ToUInt32(data, 0) != expectedMagic)
		{
			return false;
		}
		uint headerVersion = BitConverter.ToUInt32(data, 4);
		if (headerVersion < 65536 || headerVersion > 66304)
		{
			return false;
		}
		return true;
	}

	private static bool ReadVarint(BinaryReader input, out uint value)
	{
		uint v = 0u;
		int shift = 0;
		while (input.BaseStream.Position < input.BaseStream.Length)
		{
			byte b = input.ReadByte();
			v |= (uint)((b & 0x7F) << shift);
			shift += 7;
			if ((b & 0x80) == 0)
			{
				break;
			}
		}
		value = v;
		return true;
	}

	private static bool ReadLengthOp(BinaryReader input, out uint len, out SpvOp op)
	{
		len = 0u;
		op = SpvOp.Nop;
		if (!ReadVarint(input, out var value))
		{
			return false;
		}
		len = (value >> 20 << 4) | ((value >> 4) & 0xFu);
		op = (SpvOp)(((int)(value >> 4) & 0xFFF0) | ((int)value & 0xF));
		op = RemapOp(op);
		len = DecodeLen(op, len);
		return true;
	}

	private static SpvOp RemapOp(SpvOp op)
	{
		return op switch
		{
			SpvOp.Decorate => SpvOp.Nop, 
			SpvOp.Nop => SpvOp.Decorate, 
			SpvOp.Load => SpvOp.Undef, 
			SpvOp.Undef => SpvOp.Load, 
			SpvOp.Store => SpvOp.SourceContinued, 
			SpvOp.SourceContinued => SpvOp.Store, 
			SpvOp.AccessChain => SpvOp.Source, 
			SpvOp.Source => SpvOp.AccessChain, 
			SpvOp.VectorShuffle => SpvOp.SourceExtension, 
			SpvOp.SourceExtension => SpvOp.VectorShuffle, 
			SpvOp.MemberDecorate => SpvOp.String, 
			SpvOp.String => SpvOp.MemberDecorate, 
			SpvOp.Label => SpvOp.Line, 
			SpvOp.Line => SpvOp.Label, 
			SpvOp.Variable => (SpvOp)9, 
			(SpvOp)9 => SpvOp.Variable, 
			SpvOp.FMul => SpvOp.Extension, 
			SpvOp.Extension => SpvOp.FMul, 
			SpvOp.FAdd => SpvOp.ExtInstImport, 
			SpvOp.ExtInstImport => SpvOp.FAdd, 
			SpvOp.TypePointer => SpvOp.MemoryModel, 
			SpvOp.MemoryModel => SpvOp.TypePointer, 
			SpvOp.FNegate => SpvOp.EntryPoint, 
			SpvOp.EntryPoint => SpvOp.FNegate, 
			_ => op, 
		};
	}

	private static uint DecodeLen(SpvOp op, uint len)
	{
		len++;
		switch (op)
		{
		case SpvOp.VectorShuffle:
			len += 4;
			break;
		case SpvOp.VectorShuffleCompact:
			len += 4;
			break;
		case SpvOp.Decorate:
			len += 2;
			break;
		case SpvOp.Load:
			len += 3;
			break;
		case SpvOp.AccessChain:
			len += 3;
			break;
		}
		return len;
	}

	private static int DecorationExtraOps(int dec)
	{
		switch (dec)
		{
		case 0:
		case 2:
		case 3:
		case 4:
		case 5:
			return 0;
		default:
			if (dec >= 29 && dec <= 37)
			{
				return 1;
			}
			return -1;
		}
	}

	private static int ZigDecode(uint u)
	{
		if ((u & 1) == 0)
		{
			return (int)(u >> 1);
		}
		return (int)(~(u >> 1));
	}
}
