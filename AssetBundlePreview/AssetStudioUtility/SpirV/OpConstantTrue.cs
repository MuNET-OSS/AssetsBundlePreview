using System.Collections.Generic;

namespace SpirV;

public class OpConstantTrue : Instruction
{
	public OpConstantTrue()
		: base("OpConstantTrue", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
