using System.Text;

namespace SpirV;

public class FloatingPointType : ScalarType
{
	public int Width { get; }

	public FloatingPointType(int width)
	{
		Width = width;
	}

	public override string ToString()
	{
		return $"f{Width}";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append('f').Append(Width);
	}
}
