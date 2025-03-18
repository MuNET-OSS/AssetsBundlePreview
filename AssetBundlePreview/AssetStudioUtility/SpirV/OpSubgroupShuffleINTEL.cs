using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupShuffleINTEL : Instruction
{
	public OpSubgroupShuffleINTEL()
		: base("OpSubgroupShuffleINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Data", OperandQuantifier.Default),
			new Operand(new IdRef(), "InvocationId", OperandQuantifier.Default)
		})
	{
	}
}
