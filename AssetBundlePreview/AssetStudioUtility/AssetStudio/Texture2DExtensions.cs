using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AssetStudio;

public static class Texture2DExtensions
{
	public static Image<Bgra32> ConvertToImage(this Texture2D m_Texture2D, bool flip)
	{
		Texture2DConverter converter = new Texture2DConverter(m_Texture2D);
		byte[] buff = BigArrayPool<byte>.Shared.Rent(m_Texture2D.m_Width * m_Texture2D.m_Height * 4);
		try
		{
			if (converter.DecodeTexture2D(buff))
			{
				Image<Bgra32> image = Image.LoadPixelData<Bgra32>(buff, m_Texture2D.m_Width, m_Texture2D.m_Height);
				if (flip)
				{
					ProcessingExtensions.Mutate<Bgra32>(image, (Action<IImageProcessingContext>)delegate(IImageProcessingContext x)
					{
						FlipExtensions.Flip(x, (FlipMode)2);
					});
				}
				return image;
			}
			return null;
		}
		finally
		{
			BigArrayPool<byte>.Shared.Return(buff);
		}
	}
}
