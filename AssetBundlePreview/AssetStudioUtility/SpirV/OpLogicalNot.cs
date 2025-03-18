using System.Collections.Generic;

namespace SpirV;

public class OpLogicalNot : Instruction
{
	public OpLogicalNot()
		: base("OpLogicalNot", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand", OperandQuantifier.Default)
		})
	{
	}
}
