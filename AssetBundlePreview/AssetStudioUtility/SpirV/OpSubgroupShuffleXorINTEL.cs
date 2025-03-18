using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupShuffleXorINTEL : Instruction
{
	public OpSubgroupShuffleXorINTEL()
		: base("OpSubgroupShuffleXorINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Data", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
