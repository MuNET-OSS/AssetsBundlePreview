using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupAllKHR : Instruction
{
	public OpSubgroupAllKHR()
		: base("OpSubgroupAllKHR", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Predicate", OperandQuantifier.Default)
		})
	{
	}
}
