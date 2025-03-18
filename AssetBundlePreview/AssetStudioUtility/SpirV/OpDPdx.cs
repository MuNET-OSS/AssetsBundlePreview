using System.Collections.Generic;

namespace SpirV;

public class OpDPdx : Instruction
{
	public OpDPdx()
		: base("OpDPdx", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "P", OperandQuantifier.Default)
		})
	{
	}
}
