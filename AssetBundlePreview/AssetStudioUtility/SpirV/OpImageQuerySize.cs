using System.Collections.Generic;

namespace SpirV;

public class OpImageQuerySize : Instruction
{
	public OpImageQuerySize()
		: base("OpImageQuerySize", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default)
		})
	{
	}
}
