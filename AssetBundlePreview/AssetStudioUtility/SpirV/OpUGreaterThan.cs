using System.Collections.Generic;

namespace SpirV;

public class OpUGreaterThan : Instruction
{
	public OpUGreaterThan()
		: base("OpUGreaterThan", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand 1", OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand 2", OperandQuantifier.Default)
		})
	{
	}
}
