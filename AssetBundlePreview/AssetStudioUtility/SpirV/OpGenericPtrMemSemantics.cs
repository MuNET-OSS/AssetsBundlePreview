using System.Collections.Generic;

namespace SpirV;

public class OpGenericPtrMemSemantics : Instruction
{
	public OpGenericPtrMemSemantics()
		: base("OpGenericPtrMemSemantics", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default)
		})
	{
	}
}
