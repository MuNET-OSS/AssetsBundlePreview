using System.Collections.Generic;

namespace SpirV;

public class OpPtrCastToGeneric : Instruction
{
	public OpPtrCastToGeneric()
		: base("OpPtrCastToGeneric", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default)
		})
	{
	}
}
