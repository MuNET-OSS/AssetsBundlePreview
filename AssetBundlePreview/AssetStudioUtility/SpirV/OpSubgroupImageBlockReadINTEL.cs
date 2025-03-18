using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupImageBlockReadINTEL : Instruction
{
	public OpSubgroupImageBlockReadINTEL()
		: base("OpSubgroupImageBlockReadINTEL", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default)
		})
	{
	}
}
