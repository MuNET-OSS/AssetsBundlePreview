using System.Collections.Generic;

namespace SpirV;

public class OpSizeOf : Instruction
{
	public OpSizeOf()
		: base("OpSizeOf", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default)
		})
	{
	}
}
