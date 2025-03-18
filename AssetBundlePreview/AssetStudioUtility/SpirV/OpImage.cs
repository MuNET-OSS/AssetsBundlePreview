using System.Collections.Generic;

namespace SpirV;

public class OpImage : Instruction
{
	public OpImage()
		: base("OpImage", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampled Image", OperandQuantifier.Default)
		})
	{
	}
}
