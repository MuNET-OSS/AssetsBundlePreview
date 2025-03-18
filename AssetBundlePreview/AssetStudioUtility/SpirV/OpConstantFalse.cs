using System.Collections.Generic;

namespace SpirV;

public class OpConstantFalse : Instruction
{
	public OpConstantFalse()
		: base("OpConstantFalse", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
