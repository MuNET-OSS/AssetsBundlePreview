using System.Collections.Generic;

namespace SpirV;

public class OpIsNormal : Instruction
{
	public OpIsNormal()
		: base("OpIsNormal", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "x", OperandQuantifier.Default)
		})
	{
	}
}
