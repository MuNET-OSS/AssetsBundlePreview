using System.Text;

namespace SpirV;

public class VectorType : Type
{
	public ScalarType ComponentType { get; }

	public int ComponentCount { get; }

	public VectorType(ScalarType scalarType, int componentCount)
	{
		ComponentType = scalarType;
		ComponentCount = componentCount;
	}

	public override string ToString()
	{
		return $"{ComponentType}_{ComponentCount}";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return ComponentType.ToString(sb).Append('_').Append(ComponentCount);
	}
}
