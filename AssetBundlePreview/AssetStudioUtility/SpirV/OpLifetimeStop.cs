using System.Collections.Generic;

namespace SpirV;

public class OpLifetimeStop : Instruction
{
	public OpLifetimeStop()
		: base("OpLifetimeStop", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Size", OperandQuantifier.Default)
		})
	{
	}
}
