using System.Collections.Generic;

namespace SpirV;

public class OpTypeOpaque : Instruction
{
	public OpTypeOpaque()
		: base("OpTypeOpaque", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralString(), "The name of the opaque type.", OperandQuantifier.Default)
		})
	{
	}
}
