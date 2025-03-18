using System.Collections.Generic;

namespace SpirV;

public class OpImageQueryLod : Instruction
{
	public OpImageQueryLod()
		: base("OpImageQueryLod", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampled Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default)
		})
	{
	}
}
