using System.Collections.Generic;

namespace SpirV;

public class OpImageQueryFormat : Instruction
{
	public OpImageQueryFormat()
		: base("OpImageQueryFormat", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default)
		})
	{
	}
}
