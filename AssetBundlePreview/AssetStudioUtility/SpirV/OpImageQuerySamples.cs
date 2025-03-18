using System.Collections.Generic;

namespace SpirV;

public class OpImageQuerySamples : Instruction
{
	public OpImageQuerySamples()
		: base("OpImageQuerySamples", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default)
		})
	{
	}
}
