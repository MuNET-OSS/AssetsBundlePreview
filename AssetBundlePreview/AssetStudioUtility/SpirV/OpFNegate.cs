using System.Collections.Generic;

namespace SpirV;

public class OpFNegate : Instruction
{
	public OpFNegate()
		: base("OpFNegate", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand", OperandQuantifier.Default)
		})
	{
	}
}
