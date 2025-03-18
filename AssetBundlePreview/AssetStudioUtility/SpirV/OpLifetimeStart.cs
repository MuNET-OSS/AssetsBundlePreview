using System.Collections.Generic;

namespace SpirV;

public class OpLifetimeStart : Instruction
{
	public OpLifetimeStart()
		: base("OpLifetimeStart", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Size", OperandQuantifier.Default)
		})
	{
	}
}
