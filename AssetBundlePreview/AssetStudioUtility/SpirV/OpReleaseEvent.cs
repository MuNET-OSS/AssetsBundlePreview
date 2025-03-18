using System.Collections.Generic;

namespace SpirV;

public class OpReleaseEvent : Instruction
{
	public OpReleaseEvent()
		: base("OpReleaseEvent", new List<Operand>
		{
			new Operand(new IdRef(), "Event", OperandQuantifier.Default)
		})
	{
	}
}
