using System.Collections.Generic;

namespace SpirV;

public class OpMemoryBarrier : Instruction
{
	public OpMemoryBarrier()
		: base("OpMemoryBarrier", new List<Operand>
		{
			new Operand(new IdScope(), "Memory", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default)
		})
	{
	}
}
