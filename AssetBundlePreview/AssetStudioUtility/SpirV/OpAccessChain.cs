using System.Collections.Generic;

namespace SpirV;

public class OpAccessChain : Instruction
{
	public OpAccessChain()
		: base("OpAccessChain", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Indexes", OperandQuantifier.Varying)
		})
	{
	}
}
