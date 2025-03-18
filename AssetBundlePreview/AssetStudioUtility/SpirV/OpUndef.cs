using System.Collections.Generic;

namespace SpirV;

public class OpUndef : Instruction
{
	public OpUndef()
		: base("OpUndef", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
