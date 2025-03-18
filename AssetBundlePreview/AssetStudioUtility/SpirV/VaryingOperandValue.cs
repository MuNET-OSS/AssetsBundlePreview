using System.Collections.Generic;
using System.Text;

namespace SpirV;

public class VaryingOperandValue
{
	public IReadOnlyList<object> Values { get; }

	public VaryingOperandValue(IReadOnlyList<object> values)
	{
		Values = values;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		ToString(sb);
		return sb.ToString();
	}

	public StringBuilder ToString(StringBuilder sb)
	{
		for (int i = 0; i < Values.Count; i++)
		{
			if (Values[i] is ObjectReference objRef)
			{
				objRef.ToString(sb);
			}
			else
			{
				sb.Append(Values[i]);
			}
			if (i < Values.Count - 1)
			{
				sb.Append(' ');
			}
		}
		return sb;
	}
}
