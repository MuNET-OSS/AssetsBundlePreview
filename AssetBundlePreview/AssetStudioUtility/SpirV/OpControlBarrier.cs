using System.Collections.Generic;

namespace SpirV;

public class OpControlBarrier : Instruction
{
	public OpControlBarrier()
		: base("OpControlBarrier", new List<Operand>
		{
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdScope(), "Memory", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default)
		})
	{
	}
}
