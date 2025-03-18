using System.Collections.Generic;

namespace SpirV;

public class OpTypeFloat : Instruction
{
	public OpTypeFloat()
		: base("OpTypeFloat", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Width", OperandQuantifier.Default)
		})
	{
	}
}
