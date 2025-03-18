using System.Collections.Generic;

namespace SpirV;

public class OpTypeSampler : Instruction
{
	public OpTypeSampler()
		: base("OpTypeSampler", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
