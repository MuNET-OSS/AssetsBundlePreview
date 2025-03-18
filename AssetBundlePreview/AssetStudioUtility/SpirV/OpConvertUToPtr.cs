using System.Collections.Generic;

namespace SpirV;

public class OpConvertUToPtr : Instruction
{
	public OpConvertUToPtr()
		: base("OpConvertUToPtr", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Integer Value", OperandQuantifier.Default)
		})
	{
	}
}
