using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupBlockReadINTEL : Instruction
{
	public OpSubgroupBlockReadINTEL()
		: base("OpSubgroupBlockReadINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Ptr", OperandQuantifier.Default)
		})
	{
	}
}
