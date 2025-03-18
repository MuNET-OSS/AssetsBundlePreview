using System.Collections.Generic;

namespace SpirV;

public class OpGroupWaitEvents : Instruction
{
	public OpGroupWaitEvents()
		: base("OpGroupWaitEvents", new List<Operand>
		{
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdRef(), "Num Events", OperandQuantifier.Default),
			new Operand(new IdRef(), "Events List", OperandQuantifier.Default)
		})
	{
	}
}
