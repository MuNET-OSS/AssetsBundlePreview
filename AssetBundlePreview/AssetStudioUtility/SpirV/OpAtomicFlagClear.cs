using System.Collections.Generic;

namespace SpirV;

public class OpAtomicFlagClear : Instruction
{
	public OpAtomicFlagClear()
		: base("OpAtomicFlagClear", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdScope(), "Scope", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default)
		})
	{
	}
}
