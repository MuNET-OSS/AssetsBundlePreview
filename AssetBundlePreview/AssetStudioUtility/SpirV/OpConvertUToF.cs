using System.Collections.Generic;

namespace SpirV;

public class OpConvertUToF : Instruction
{
	public OpConvertUToF()
		: base("OpConvertUToF", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Unsigned Value", OperandQuantifier.Default)
		})
	{
	}
}
