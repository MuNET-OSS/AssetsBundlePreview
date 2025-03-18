using System.Collections.Generic;

namespace SpirV;

public class OpGroupAny : Instruction
{
	public OpGroupAny()
		: base("OpGroupAny", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdRef(), "Predicate", OperandQuantifier.Default)
		})
	{
	}
}
