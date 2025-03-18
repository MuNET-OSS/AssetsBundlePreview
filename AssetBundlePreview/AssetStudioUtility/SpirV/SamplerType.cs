using System.Text;

namespace SpirV;

public class SamplerType : Type
{
	public override string ToString()
	{
		return "sampler";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append("sampler");
	}
}
