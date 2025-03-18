using System.Collections.Generic;

namespace SpirV;

public class OpAtomicSMax : Instruction
{
	public OpAtomicSMax()
		: base("OpAtomicSMax", new List<Operand>
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
