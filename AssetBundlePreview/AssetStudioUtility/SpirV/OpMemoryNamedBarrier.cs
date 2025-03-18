using System.Collections.Generic;

namespace SpirV;

public class OpMemoryNamedBarrier : Instruction
{
	public OpMemoryNamedBarrier()
		: base("OpMemoryNamedBarrier", new List<Operand>
		{
			new Operand(new IdRef(), "Named Barrier", OperandQuantifier.Default),
			new Operand(new IdScope(), "Memory", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default)
		})
	{
	}
}
