using System.Collections.Generic;

namespace SpirV;

public class OpUnordered : Instruction
{
	public OpUnordered()
		: base("OpUnordered", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "x", OperandQuantifier.Default),
			new Operand(new IdRef(), "y", OperandQuantifier.Default)
		})
	{
	}
}
