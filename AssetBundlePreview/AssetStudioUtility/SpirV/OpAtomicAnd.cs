using System.Collections.Generic;

namespace SpirV;

public class OpAtomicAnd : Instruction
{
	public OpAtomicAnd()
		: base("OpAtomicAnd", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdScope(), "Scope", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Semantics", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
