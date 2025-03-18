using System.Text;

namespace SpirV;

public class ImageType : Type
{
	public Type SampledType { get; }

	public Dim Dim { get; }

	public int Depth { get; }

	public bool IsArray { get; }

	public bool IsMultisampled { get; }

	public int SampleCount { get; }

	public ImageFormat Format { get; }

	public AccessQualifier AccessQualifier { get; }

	public ImageType(Type sampledType, Dim dim, int depth, bool isArray, bool isMultisampled, int sampleCount, ImageFormat imageFormat, AccessQualifier accessQualifier)
	{
		SampledType = sampledType;
		Dim = dim;
		Depth = depth;
		IsArray = isArray;
		IsMultisampled = isMultisampled;
		SampleCount = sampleCount;
		Format = imageFormat;
		AccessQualifier = accessQualifier;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		ToString(sb);
		return sb.ToString();
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		switch (AccessQualifier)
		{
		case AccessQualifier.ReadWrite:
			sb.Append("read_write ");
			break;
		case AccessQualifier.WriteOnly:
			sb.Append("write_only ");
			break;
		case AccessQualifier.ReadOnly:
			sb.Append("read_only ");
			break;
		}
		sb.Append("Texture");
		switch (Dim)
		{
		case Dim.Dim1D:
			sb.Append("1D");
			break;
		case Dim.Dim2D:
			sb.Append("2D");
			break;
		case Dim.Dim3D:
			sb.Append("3D");
			break;
		case Dim.Cube:
			sb.Append("Cube");
			break;
		}
		if (IsMultisampled)
		{
			sb.Append("MS");
		}
		if (IsArray)
		{
			sb.Append("Array");
		}
		return sb;
	}
}
