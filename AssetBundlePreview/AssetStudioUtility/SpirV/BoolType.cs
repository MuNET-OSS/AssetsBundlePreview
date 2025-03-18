using System.Text;

namespace SpirV;

public class BoolType : ScalarType
{
	public override string ToString()
	{
		return "bool";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append("bool");
	}
}
