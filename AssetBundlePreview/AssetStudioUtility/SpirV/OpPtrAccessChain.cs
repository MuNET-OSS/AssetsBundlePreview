using System.Collections.Generic;

namespace SpirV;

public class OpPtrAccessChain : Instruction
{
	public OpPtrAccessChain()
		: base("OpPtrAccessChain", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Element", OperandQuantifier.Default),
			new Operand(new IdRef(), "Indexes", OperandQuantifier.Varying)
		})
	{
	}
}
