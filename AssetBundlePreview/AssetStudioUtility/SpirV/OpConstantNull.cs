using System.Collections.Generic;

namespace SpirV;

public class OpConstantNull : Instruction
{
	public OpConstantNull()
		: base("OpConstantNull", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
