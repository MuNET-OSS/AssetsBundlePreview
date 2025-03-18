using System.Collections.Generic;

namespace SpirV;

public class OpGetDefaultQueue : Instruction
{
	public OpGetDefaultQueue()
		: base("OpGetDefaultQueue", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
