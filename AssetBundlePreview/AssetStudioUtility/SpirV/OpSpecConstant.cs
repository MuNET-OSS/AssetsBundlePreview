using System.Collections.Generic;

namespace SpirV;

public class OpSpecConstant : Instruction
{
	public OpSpecConstant()
		: base("OpSpecConstant", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralContextDependentNumber(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
