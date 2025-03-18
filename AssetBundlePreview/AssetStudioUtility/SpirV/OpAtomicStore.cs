using System.Collections.Generic;

namespace SpirV;

public class OpAtomicStore : Instruction
{
	public OpAtomicStore()
		: base("OpAtomicStore", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdScope(), "Scope", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
