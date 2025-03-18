using System.Collections.Generic;

namespace SpirV;

public class OpOuterProduct : Instruction
{
	public OpOuterProduct()
		: base("OpOuterProduct", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector 1", OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector 2", OperandQuantifier.Default)
		})
	{
	}
}
