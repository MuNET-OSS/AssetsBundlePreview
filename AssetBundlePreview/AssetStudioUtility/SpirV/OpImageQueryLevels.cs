using System.Collections.Generic;

namespace SpirV;

public class OpImageQueryLevels : Instruction
{
	public OpImageQueryLevels()
		: base("OpImageQueryLevels", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default)
		})
	{
	}
}
