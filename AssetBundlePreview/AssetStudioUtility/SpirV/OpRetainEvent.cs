using System.Collections.Generic;

namespace SpirV;

public class OpRetainEvent : Instruction
{
	public OpRetainEvent()
		: base("OpRetainEvent", new List<Operand>
		{
			new Operand(new IdRef(), "Event", OperandQuantifier.Default)
		})
	{
	}
}
