using System.Text;

namespace SpirV;

public class VoidType : Type
{
	public override string ToString()
	{
		return "void";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append("void");
	}
}
