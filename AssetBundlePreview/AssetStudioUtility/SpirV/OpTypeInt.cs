using System.Collections.Generic;

namespace SpirV;

public class OpTypeInt : Instruction
{
	public OpTypeInt()
		: base("OpTypeInt", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Width", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Signedness", OperandQuantifier.Default)
		})
	{
	}
}
