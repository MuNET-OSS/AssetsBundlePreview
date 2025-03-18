using System.Collections.Generic;

namespace SpirV;

public class OpImageQuerySizeLod : Instruction
{
	public OpImageQuerySizeLod()
		: base("OpImageQuerySizeLod", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Level of Detail", OperandQuantifier.Default)
		})
	{
	}
}
