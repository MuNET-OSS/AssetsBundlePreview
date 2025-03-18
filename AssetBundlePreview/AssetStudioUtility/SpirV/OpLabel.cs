using System.Collections.Generic;

namespace SpirV;

public class OpLabel : Instruction
{
	public OpLabel()
		: base("OpLabel", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
