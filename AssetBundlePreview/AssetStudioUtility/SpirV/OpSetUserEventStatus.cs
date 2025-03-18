using System.Collections.Generic;

namespace SpirV;

public class OpSetUserEventStatus : Instruction
{
	public OpSetUserEventStatus()
		: base("OpSetUserEventStatus", new List<Operand>
		{
			new Operand(new IdRef(), "Event", OperandQuantifier.Default),
			new Operand(new IdRef(), "Status", OperandQuantifier.Default)
		})
	{
	}
}
