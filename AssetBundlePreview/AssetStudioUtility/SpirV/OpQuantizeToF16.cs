using System.Collections.Generic;

namespace SpirV;

public class OpQuantizeToF16 : Instruction
{
	public OpQuantizeToF16()
		: base("OpQuantizeToF16", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
