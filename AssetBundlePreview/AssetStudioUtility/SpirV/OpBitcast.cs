using System.Collections.Generic;

namespace SpirV;

public class OpBitcast : Instruction
{
	public OpBitcast()
		: base("OpBitcast", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand", OperandQuantifier.Default)
		})
	{
	}
}
