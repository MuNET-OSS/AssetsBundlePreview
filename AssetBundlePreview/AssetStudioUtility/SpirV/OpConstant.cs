using System.Collections.Generic;

namespace SpirV;

public class OpConstant : Instruction
{
	public OpConstant()
		: base("OpConstant", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralContextDependentNumber(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
