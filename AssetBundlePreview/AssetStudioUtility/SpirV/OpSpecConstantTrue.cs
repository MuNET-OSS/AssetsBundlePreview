using System.Collections.Generic;

namespace SpirV;

public class OpSpecConstantTrue : Instruction
{
	public OpSpecConstantTrue()
		: base("OpSpecConstantTrue", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
