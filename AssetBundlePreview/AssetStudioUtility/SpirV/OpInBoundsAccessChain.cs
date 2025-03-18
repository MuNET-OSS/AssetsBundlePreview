using System.Collections.Generic;

namespace SpirV;

public class OpInBoundsAccessChain : Instruction
{
	public OpInBoundsAccessChain()
		: base("OpInBoundsAccessChain", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Indexes", OperandQuantifier.Varying)
		})
	{
	}
}
