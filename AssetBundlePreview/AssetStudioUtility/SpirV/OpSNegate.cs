using System.Collections.Generic;

namespace SpirV;

public class OpSNegate : Instruction
{
	public OpSNegate()
		: base("OpSNegate", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand", OperandQuantifier.Default)
		})
	{
	}
}
