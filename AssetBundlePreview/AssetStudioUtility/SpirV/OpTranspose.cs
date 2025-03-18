using System.Collections.Generic;

namespace SpirV;

public class OpTranspose : Instruction
{
	public OpTranspose()
		: base("OpTranspose", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Matrix", OperandQuantifier.Default)
		})
	{
	}
}
