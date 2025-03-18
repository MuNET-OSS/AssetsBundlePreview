using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupBlockWriteINTEL : Instruction
{
	public OpSubgroupBlockWriteINTEL()
		: base("OpSubgroupBlockWriteINTEL", new List<Operand>
		{
			new Operand(new IdRef(), "Ptr", OperandQuantifier.Default),
			new Operand(new IdRef(), "Data", OperandQuantifier.Default)
		})
	{
	}
}
