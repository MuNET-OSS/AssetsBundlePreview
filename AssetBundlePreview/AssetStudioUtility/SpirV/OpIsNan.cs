using System.Collections.Generic;

namespace SpirV;

public class OpIsNan : Instruction
{
	public OpIsNan()
		: base("OpIsNan", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "x", OperandQuantifier.Default)
		})
	{
	}
}
