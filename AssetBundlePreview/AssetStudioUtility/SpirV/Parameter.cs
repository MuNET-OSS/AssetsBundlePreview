using System.Collections.Generic;

namespace SpirV;

public class Parameter
{
	public virtual IReadOnlyList<OperandType> OperandTypes { get; }
}
