using System.Collections.Generic;

namespace SpirV;

public class OpEnqueueMarker : Instruction
{
	public OpEnqueueMarker()
		: base("OpEnqueueMarker", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Queue", OperandQuantifier.Default),
			new Operand(new IdRef(), "Num Events", OperandQuantifier.Default),
			new Operand(new IdRef(), "Wait Events", OperandQuantifier.Default),
			new Operand(new IdRef(), "Ret Event", OperandQuantifier.Default)
		})
	{
	}
}
