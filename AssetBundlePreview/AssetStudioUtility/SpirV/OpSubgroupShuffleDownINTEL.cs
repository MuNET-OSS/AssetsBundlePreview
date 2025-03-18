using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupShuffleDownINTEL : Instruction
{
	public OpSubgroupShuffleDownINTEL()
		: base("OpSubgroupShuffleDownINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Current", OperandQuantifier.Default),
			new Operand(new IdRef(), "Next", OperandQuantifier.Default),
			new Operand(new IdRef(), "Delta", OperandQuantifier.Default)
		})
	{
	}
}
