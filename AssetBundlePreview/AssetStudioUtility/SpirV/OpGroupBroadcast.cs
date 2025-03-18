using System.Collections.Generic;

namespace SpirV;

public class OpGroupBroadcast : Instruction
{
	public OpGroupBroadcast()
		: base("OpGroupBroadcast", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default),
			new Operand(new IdRef(), "LocalId", OperandQuantifier.Default)
		})
	{
	}
}
