using System.Collections.Generic;

namespace SpirV;

public class OpTypeVector : Instruction
{
	public OpTypeVector()
		: base("OpTypeVector", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Component Type", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Component Count", OperandQuantifier.Default)
		})
	{
	}
}
