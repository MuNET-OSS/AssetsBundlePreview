using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupAnyKHR : Instruction
{
	public OpSubgroupAnyKHR()
		: base("OpSubgroupAnyKHR", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Predicate", OperandQuantifier.Default)
		})
	{
	}
}
