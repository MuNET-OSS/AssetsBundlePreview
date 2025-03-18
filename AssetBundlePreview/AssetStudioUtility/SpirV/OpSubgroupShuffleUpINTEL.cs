using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupShuffleUpINTEL : Instruction
{
	public OpSubgroupShuffleUpINTEL()
		: base("OpSubgroupShuffleUpINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Previous", OperandQuantifier.Default),
			new Operand(new IdRef(), "Current", OperandQuantifier.Default),
			new Operand(new IdRef(), "Delta", OperandQuantifier.Default)
		})
	{
	}
}
