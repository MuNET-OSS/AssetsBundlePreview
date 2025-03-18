using System.Collections.Generic;

namespace SpirV;

public class OpOrdered : Instruction
{
	public OpOrdered()
		: base("OpOrdered", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "x", OperandQuantifier.Default),
			new Operand(new IdRef(), "y", OperandQuantifier.Default)
		})
	{
	}
}
