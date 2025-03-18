using System.Collections.Generic;

namespace SpirV;

public class OpTypeQueue : Instruction
{
	public OpTypeQueue()
		: base("OpTypeQueue", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
