using System.Collections.Generic;

namespace SpirV;

public class OpAll : Instruction
{
	public OpAll()
		: base("OpAll", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector", OperandQuantifier.Default)
		})
	{
	}
}
