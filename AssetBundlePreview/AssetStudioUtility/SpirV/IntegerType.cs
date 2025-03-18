using System.Text;

namespace SpirV;

public class IntegerType : ScalarType
{
	public int Width { get; }

	public bool Signed { get; }

	public IntegerType(int width, bool signed)
	{
		Width = width;
		Signed = signed;
	}

	public override string ToString()
	{
		if (Signed)
		{
			return $"i{Width}";
		}
		return $"u{Width}";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		if (Signed)
		{
			sb.Append('i').Append(Width);
		}
		else
		{
			sb.Append('u').Append(Width);
		}
		return sb;
	}
}
