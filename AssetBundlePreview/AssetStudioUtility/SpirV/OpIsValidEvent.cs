using System.Collections.Generic;

namespace SpirV;

public class OpIsValidEvent : Instruction
{
	public OpIsValidEvent()
		: base("OpIsValidEvent", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Event", OperandQuantifier.Default)
		})
	{
	}
}
