using System.Text;

namespace SpirV;

public class SampledImageType : Type
{
	public ImageType ImageType { get; }

	public SampledImageType(ImageType imageType)
	{
		ImageType = imageType;
	}

	public override string ToString()
	{
		return $"{ImageType}Sampled";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return ImageType.ToString(sb).Append("Sampled");
	}
}
