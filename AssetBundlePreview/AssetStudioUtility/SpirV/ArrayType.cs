using System.Text;

namespace SpirV;

public class ArrayType : Type
{
	public int ElementCount { get; }

	public Type ElementType { get; }

	public ArrayType(Type elementType, int elementCount)
	{
		ElementType = elementType;
		ElementCount = elementCount;
	}

	public override string ToString()
	{
		return $"{ElementType}[{ElementCount}]";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		return ElementType.ToString(sb).Append('[').Append(ElementCount)
			.Append(']');
	}
}
