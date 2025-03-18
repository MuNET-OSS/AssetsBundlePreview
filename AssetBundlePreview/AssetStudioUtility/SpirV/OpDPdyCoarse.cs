using System.Collections.Generic;

namespace SpirV;

public class OpDPdyCoarse : Instruction
{
	public OpDPdyCoarse()
		: base("OpDPdyCoarse", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "P", OperandQuantifier.Default)
		})
	{
	}
}
