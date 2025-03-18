using System.Collections.Generic;

namespace SpirV;

public class OpCreateUserEvent : Instruction
{
	public OpCreateUserEvent()
		: base("OpCreateUserEvent", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
