using System.Collections.Generic;

namespace SpirV;

public class OpGenericCastToPtr : Instruction
{
	public OpGenericCastToPtr()
		: base("OpGenericCastToPtr", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default)
		})
	{
	}
}
