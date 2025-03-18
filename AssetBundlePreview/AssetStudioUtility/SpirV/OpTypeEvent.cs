using System.Collections.Generic;

namespace SpirV;

public class OpTypeEvent : Instruction
{
	public OpTypeEvent()
		: base("OpTypeEvent", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
