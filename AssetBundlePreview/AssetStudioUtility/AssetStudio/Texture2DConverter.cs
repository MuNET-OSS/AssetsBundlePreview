using System;
using System.Runtime.CompilerServices;
using Texture2DDecoder;

namespace AssetStudio;

public class Texture2DConverter
{
	private ResourceReader reader;

	private int m_Width;

	private int m_Height;

	private TextureFormat m_TextureFormat;

	private int[] version;

	private BuildTarget platform;

	private int outPutSize;

	public Texture2DConverter(Texture2D m_Texture2D)
	{
		reader = m_Texture2D.image_data;
		m_Width = m_Texture2D.m_Width;
		m_Height = m_Texture2D.m_Height;
		m_TextureFormat = m_Texture2D.m_TextureFormat;
		version = m_Texture2D.version;
		platform = m_Texture2D.platform;
		outPutSize = m_Width * m_Height * 4;
	}

	public bool DecodeTexture2D(byte[] bytes)
	{
		if (reader.Size == 0 || m_Width == 0 || m_Height == 0)
		{
			return false;
		}
		bool flag = false;
		byte[] buff = BigArrayPool<byte>.Shared.Rent(reader.Size);
		reader.GetData(buff);
		switch (m_TextureFormat)
		{
		case TextureFormat.Alpha8:
			flag = DecodeAlpha8(buff, bytes);
			break;
		case TextureFormat.ARGB4444:
			SwapBytesForXbox(buff);
			flag = DecodeARGB4444(buff, bytes);
			break;
		case TextureFormat.RGB24:
			flag = DecodeRGB24(buff, bytes);
			break;
		case TextureFormat.RGBA32:
			flag = DecodeRGBA32(buff, bytes);
			break;
		case TextureFormat.ARGB32:
			flag = DecodeARGB32(buff, bytes);
			break;
		case TextureFormat.RGB565:
			SwapBytesForXbox(buff);
			flag = DecodeRGB565(buff, bytes);
			break;
		case TextureFormat.R16:
			flag = DecodeR16(buff, bytes);
			break;
		case TextureFormat.DXT1:
			SwapBytesForXbox(buff);
			flag = DecodeDXT1(buff, bytes);
			break;
		case TextureFormat.DXT5:
			SwapBytesForXbox(buff);
			flag = DecodeDXT5(buff, bytes);
			break;
		case TextureFormat.RGBA4444:
			flag = DecodeRGBA4444(buff, bytes);
			break;
		case TextureFormat.BGRA32:
			flag = DecodeBGRA32(buff, bytes);
			break;
		case TextureFormat.RHalf:
			flag = DecodeRHalf(buff, bytes);
			break;
		case TextureFormat.RGHalf:
			flag = DecodeRGHalf(buff, bytes);
			break;
		case TextureFormat.RGBAHalf:
			flag = DecodeRGBAHalf(buff, bytes);
			break;
		case TextureFormat.RFloat:
			flag = DecodeRFloat(buff, bytes);
			break;
		case TextureFormat.RGFloat:
			flag = DecodeRGFloat(buff, bytes);
			break;
		case TextureFormat.RGBAFloat:
			flag = DecodeRGBAFloat(buff, bytes);
			break;
		case TextureFormat.YUY2:
			flag = DecodeYUY2(buff, bytes);
			break;
		case TextureFormat.RGB9e5Float:
			flag = DecodeRGB9e5Float(buff, bytes);
			break;
		case TextureFormat.BC6H:
			flag = DecodeBC6H(buff, bytes);
			break;
		case TextureFormat.BC7:
			flag = DecodeBC7(buff, bytes);
			break;
		case TextureFormat.BC4:
			flag = DecodeBC4(buff, bytes);
			break;
		case TextureFormat.BC5:
			flag = DecodeBC5(buff, bytes);
			break;
		case TextureFormat.DXT1Crunched:
			flag = DecodeDXT1Crunched(buff, bytes);
			break;
		case TextureFormat.DXT5Crunched:
			flag = DecodeDXT5Crunched(buff, bytes);
			break;
		case TextureFormat.PVRTC_RGB2:
		case TextureFormat.PVRTC_RGBA2:
			flag = DecodePVRTC(buff, bytes, is2bpp: true);
			break;
		case TextureFormat.PVRTC_RGB4:
		case TextureFormat.PVRTC_RGBA4:
			flag = DecodePVRTC(buff, bytes, is2bpp: false);
			break;
		case TextureFormat.ETC_RGB4:
		case TextureFormat.ETC_RGB4_3DS:
			flag = DecodeETC1(buff, bytes);
			break;
		case TextureFormat.ATC_RGB4:
			flag = DecodeATCRGB4(buff, bytes);
			break;
		case TextureFormat.ATC_RGBA8:
			flag = DecodeATCRGBA8(buff, bytes);
			break;
		case TextureFormat.EAC_R:
			flag = DecodeEACR(buff, bytes);
			break;
		case TextureFormat.EAC_R_SIGNED:
			flag = DecodeEACRSigned(buff, bytes);
			break;
		case TextureFormat.EAC_RG:
			flag = DecodeEACRG(buff, bytes);
			break;
		case TextureFormat.EAC_RG_SIGNED:
			flag = DecodeEACRGSigned(buff, bytes);
			break;
		case TextureFormat.ETC2_RGB:
			flag = DecodeETC2(buff, bytes);
			break;
		case TextureFormat.ETC2_RGBA1:
			flag = DecodeETC2A1(buff, bytes);
			break;
		case TextureFormat.ETC2_RGBA8:
		case TextureFormat.ETC_RGBA8_3DS:
			flag = DecodeETC2A8(buff, bytes);
			break;
		case TextureFormat.ASTC_RGB_4x4:
		case TextureFormat.ASTC_RGBA_4x4:
		case TextureFormat.ASTC_HDR_4x4:
			flag = DecodeASTC(buff, bytes, 4);
			break;
		case TextureFormat.ASTC_RGB_5x5:
		case TextureFormat.ASTC_RGBA_5x5:
		case TextureFormat.ASTC_HDR_5x5:
			flag = DecodeASTC(buff, bytes, 5);
			break;
		case TextureFormat.ASTC_RGB_6x6:
		case TextureFormat.ASTC_RGBA_6x6:
		case TextureFormat.ASTC_HDR_6x6:
			flag = DecodeASTC(buff, bytes, 6);
			break;
		case TextureFormat.ASTC_RGB_8x8:
		case TextureFormat.ASTC_RGBA_8x8:
		case TextureFormat.ASTC_HDR_8x8:
			flag = DecodeASTC(buff, bytes, 8);
			break;
		case TextureFormat.ASTC_RGB_10x10:
		case TextureFormat.ASTC_RGBA_10x10:
		case TextureFormat.ASTC_HDR_10x10:
			flag = DecodeASTC(buff, bytes, 10);
			break;
		case TextureFormat.ASTC_RGB_12x12:
		case TextureFormat.ASTC_RGBA_12x12:
		case TextureFormat.ASTC_HDR_12x12:
			flag = DecodeASTC(buff, bytes, 12);
			break;
		case TextureFormat.RG16:
			flag = DecodeRG16(buff, bytes);
			break;
		case TextureFormat.R8:
			flag = DecodeR8(buff, bytes);
			break;
		case TextureFormat.ETC_RGB4Crunched:
			flag = DecodeETC1Crunched(buff, bytes);
			break;
		case TextureFormat.ETC2_RGBA8Crunched:
			flag = DecodeETC2A8Crunched(buff, bytes);
			break;
		case TextureFormat.RG32:
			flag = DecodeRG32(buff, bytes);
			break;
		case TextureFormat.RGB48:
			flag = DecodeRGB48(buff, bytes);
			break;
		case TextureFormat.RGBA64:
			flag = DecodeRGBA64(buff, bytes);
			break;
		}
		BigArrayPool<byte>.Shared.Return(buff);
		return flag;
	}

	private void SwapBytesForXbox(byte[] image_data)
	{
		if (platform == BuildTarget.XBOX360)
		{
			for (int i = 0; i < reader.Size / 2; i++)
			{
				byte b = image_data[i * 2];
				image_data[i * 2] = image_data[i * 2 + 1];
				image_data[i * 2 + 1] = b;
			}
		}
	}

	private bool DecodeAlpha8(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		new Span<byte>(buff).Fill(byte.MaxValue);
		for (int i = 0; i < size; i++)
		{
			buff[i * 4 + 3] = image_data[i];
		}
		return true;
	}

	private bool DecodeARGB4444(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		byte[] pixelNew = new byte[4];
		for (int i = 0; i < size; i++)
		{
			ushort pixelOldShort = BitConverter.ToUInt16(image_data, i * 2);
			pixelNew[0] = (byte)(pixelOldShort & 0xFu);
			pixelNew[1] = (byte)((pixelOldShort & 0xF0) >> 4);
			pixelNew[2] = (byte)((pixelOldShort & 0xF00) >> 8);
			pixelNew[3] = (byte)((pixelOldShort & 0xF000) >> 12);
			for (int j = 0; j < 4; j++)
			{
				pixelNew[j] = (byte)((pixelNew[j] << 4) | pixelNew[j]);
			}
			pixelNew.CopyTo(buff, i * 4);
		}
		return true;
	}

	private bool DecodeRGB24(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			buff[i * 4] = image_data[i * 3 + 2];
			buff[i * 4 + 1] = image_data[i * 3 + 1];
			buff[i * 4 + 2] = image_data[i * 3];
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGBA32(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = image_data[i + 2];
			buff[i + 1] = image_data[i + 1];
			buff[i + 2] = image_data[i];
			buff[i + 3] = image_data[i + 3];
		}
		return true;
	}

	private bool DecodeARGB32(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = image_data[i + 3];
			buff[i + 1] = image_data[i + 2];
			buff[i + 2] = image_data[i + 1];
			buff[i + 3] = image_data[i];
		}
		return true;
	}

	private bool DecodeRGB565(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			ushort p = BitConverter.ToUInt16(image_data, i * 2);
			buff[i * 4] = (byte)((uint)(p << 3) | ((uint)(p >> 2) & 7u));
			buff[i * 4 + 1] = (byte)(((uint)(p >> 3) & 0xFCu) | ((uint)(p >> 9) & 3u));
			buff[i * 4 + 2] = (byte)(((uint)(p >> 8) & 0xF8u) | (uint)(p >> 13));
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeR16(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			buff[i * 4] = 0;
			buff[i * 4 + 1] = 0;
			buff[i * 4 + 2] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 2));
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeDXT1(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeDXT1(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeDXT5(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeDXT5(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeRGBA4444(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		byte[] pixelNew = new byte[4];
		for (int i = 0; i < size; i++)
		{
			ushort pixelOldShort = BitConverter.ToUInt16(image_data, i * 2);
			pixelNew[0] = (byte)((pixelOldShort & 0xF0) >> 4);
			pixelNew[1] = (byte)((pixelOldShort & 0xF00) >> 8);
			pixelNew[2] = (byte)((pixelOldShort & 0xF000) >> 12);
			pixelNew[3] = (byte)(pixelOldShort & 0xFu);
			for (int j = 0; j < 4; j++)
			{
				pixelNew[j] = (byte)((pixelNew[j] << 4) | pixelNew[j]);
			}
			pixelNew.CopyTo(buff, i * 4);
		}
		return true;
	}

	private bool DecodeBGRA32(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = image_data[i];
			buff[i + 1] = image_data[i + 1];
			buff[i + 2] = image_data[i + 2];
			buff[i + 3] = image_data[i + 3];
		}
		return true;
	}

	private bool DecodeRHalf(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = 0;
			buff[i + 1] = 0;
			buff[i + 2] = (byte)Math.Round((float)Half.ToHalf(image_data, i / 2) * 255f);
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGHalf(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = 0;
			buff[i + 1] = (byte)Math.Round((float)Half.ToHalf(image_data, i + 2) * 255f);
			buff[i + 2] = (byte)Math.Round((float)Half.ToHalf(image_data, i) * 255f);
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGBAHalf(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = (byte)Math.Round((float)Half.ToHalf(image_data, i * 2 + 4) * 255f);
			buff[i + 1] = (byte)Math.Round((float)Half.ToHalf(image_data, i * 2 + 2) * 255f);
			buff[i + 2] = (byte)Math.Round((float)Half.ToHalf(image_data, i * 2) * 255f);
			buff[i + 3] = (byte)Math.Round((float)Half.ToHalf(image_data, i * 2 + 6) * 255f);
		}
		return true;
	}

	private bool DecodeRFloat(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = 0;
			buff[i + 1] = 0;
			buff[i + 2] = (byte)Math.Round(BitConverter.ToSingle(image_data, i) * 255f);
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGFloat(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = 0;
			buff[i + 1] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 2 + 4) * 255f);
			buff[i + 2] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 2) * 255f);
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGBAFloat(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 4 + 8) * 255f);
			buff[i + 1] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 4 + 4) * 255f);
			buff[i + 2] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 4) * 255f);
			buff[i + 3] = (byte)Math.Round(BitConverter.ToSingle(image_data, i * 4 + 12) * 255f);
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static byte ClampByte(int x)
	{
		return (byte)((255 < x) ? 255u : ((x > 0) ? ((uint)x) : 0u));
	}

	private bool DecodeYUY2(byte[] image_data, byte[] buff)
	{
		int p = 0;
		int o = 0;
		int halfWidth = m_Width / 2;
		for (int j = 0; j < m_Height; j++)
		{
			for (int i = 0; i < halfWidth; i++)
			{
				int y0 = image_data[p++];
				int u0 = image_data[p++];
				byte num = image_data[p++];
				byte num2 = image_data[p++];
				int c = y0 - 16;
				int d = u0 - 128;
				int e = num2 - 128;
				buff[o++] = ClampByte(298 * c + 516 * d + 128 >> 8);
				buff[o++] = ClampByte(298 * c - 100 * d - 208 * e + 128 >> 8);
				buff[o++] = ClampByte(298 * c + 409 * e + 128 >> 8);
				buff[o++] = byte.MaxValue;
				c = num - 16;
				buff[o++] = ClampByte(298 * c + 516 * d + 128 >> 8);
				buff[o++] = ClampByte(298 * c - 100 * d - 208 * e + 128 >> 8);
				buff[o++] = ClampByte(298 * c + 409 * e + 128 >> 8);
				buff[o++] = byte.MaxValue;
			}
		}
		return true;
	}

	private bool DecodeRGB9e5Float(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			int num = BitConverter.ToInt32(image_data, i);
			int scale = (num >> 27) & 0x1F;
			double scalef = Math.Pow(2.0, scale - 24);
			int b = (num >> 18) & 0x1FF;
			int g = (num >> 9) & 0x1FF;
			int r = num & 0x1FF;
			buff[i] = (byte)Math.Round((double)b * scalef * 255.0);
			buff[i + 1] = (byte)Math.Round((double)g * scalef * 255.0);
			buff[i + 2] = (byte)Math.Round((double)r * scalef * 255.0);
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeBC4(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeBC4(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeBC5(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeBC5(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeBC6H(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeBC6(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeBC7(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeBC7(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeDXT1Crunched(byte[] image_data, byte[] buff)
	{
		if (UnpackCrunch(image_data, out var result) && DecodeDXT1(result, buff))
		{
			return true;
		}
		return false;
	}

	private bool DecodeDXT5Crunched(byte[] image_data, byte[] buff)
	{
		if (UnpackCrunch(image_data, out var result) && DecodeDXT5(result, buff))
		{
			return true;
		}
		return false;
	}

	private bool DecodePVRTC(byte[] image_data, byte[] buff, bool is2bpp)
	{
		return TextureDecoder.DecodePVRTC(image_data, m_Width, m_Height, buff, is2bpp);
	}

	private bool DecodeETC1(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeETC1(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeATCRGB4(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeATCRGB4(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeATCRGBA8(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeATCRGBA8(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeEACR(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeEACR(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeEACRSigned(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeEACRSigned(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeEACRG(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeEACRG(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeEACRGSigned(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeEACRGSigned(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeETC2(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeETC2(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeETC2A1(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeETC2A1(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeETC2A8(byte[] image_data, byte[] buff)
	{
		return TextureDecoder.DecodeETC2A8(image_data, m_Width, m_Height, buff);
	}

	private bool DecodeASTC(byte[] image_data, byte[] buff, int blocksize)
	{
		return TextureDecoder.DecodeASTC(image_data, m_Width, m_Height, blocksize, blocksize, buff);
	}

	private bool DecodeRG16(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			buff[i * 4] = 0;
			buff[i * 4 + 1] = image_data[i * 2 + 1];
			buff[i * 4 + 2] = image_data[i * 2];
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeR8(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			buff[i * 4] = 0;
			buff[i * 4 + 1] = 0;
			buff[i * 4 + 2] = image_data[i];
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeETC1Crunched(byte[] image_data, byte[] buff)
	{
		if (UnpackCrunch(image_data, out var result) && DecodeETC1(result, buff))
		{
			return true;
		}
		return false;
	}

	private bool DecodeETC2A8Crunched(byte[] image_data, byte[] buff)
	{
		if (UnpackCrunch(image_data, out var result) && DecodeETC2A8(result, buff))
		{
			return true;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte DownScaleFrom16BitTo8Bit(ushort component)
	{
		return (byte)(component * 255 + 32895 >> 16);
	}

	private bool DecodeRG32(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = 0;
			buff[i + 1] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i + 2));
			buff[i + 2] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i));
			buff[i + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGB48(byte[] image_data, byte[] buff)
	{
		int size = m_Width * m_Height;
		for (int i = 0; i < size; i++)
		{
			buff[i * 4] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 6 + 4));
			buff[i * 4 + 1] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 6 + 2));
			buff[i * 4 + 2] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 6));
			buff[i * 4 + 3] = byte.MaxValue;
		}
		return true;
	}

	private bool DecodeRGBA64(byte[] image_data, byte[] buff)
	{
		for (int i = 0; i < outPutSize; i += 4)
		{
			buff[i] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 2 + 4));
			buff[i + 1] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 2 + 2));
			buff[i + 2] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 2));
			buff[i + 3] = DownScaleFrom16BitTo8Bit(BitConverter.ToUInt16(image_data, i * 2 + 6));
		}
		return true;
	}

	private bool UnpackCrunch(byte[] image_data, out byte[] result)
	{
		if (version[0] > 2017 || (version[0] == 2017 && version[1] >= 3) || m_TextureFormat == TextureFormat.ETC_RGB4Crunched || m_TextureFormat == TextureFormat.ETC2_RGBA8Crunched)
		{
			result = TextureDecoder.UnpackUnityCrunch(image_data);
		}
		else
		{
			result = TextureDecoder.UnpackCrunch(image_data);
		}
		if (result != null)
		{
			return true;
		}
		return false;
	}
}
