using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupImageBlockWriteINTEL : Instruction
{
	public OpSubgroupImageBlockWriteINTEL()
		: base("OpSubgroupImageBlockWriteINTEL", new List<Operand>
		{
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new IdRef(), "Data", OperandQuantifier.Default)
		})
	{
	}
}
