using System.Collections.Generic;

namespace SpirV;

public class OpAtomicCompareExchangeWeak : Instruction
{
	public OpAtomicCompareExchangeWeak()
		: base("OpAtomicCompareExchangeWeak", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdScope(), "Scope", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Equal", OperandQuantifier.Default),
			new Operand(new IdMemorySemantics(), "Unequal", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default),
			new Operand(new IdRef(), "Comparator", OperandQuantifier.Default)
		})
	{
	}
}
