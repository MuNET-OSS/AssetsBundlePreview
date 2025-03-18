using System.Collections.Generic;

namespace SpirV;

public class OpConvertSToF : Instruction
{
	public OpConvertSToF()
		: base("OpConvertSToF", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Signed Value", OperandQuantifier.Default)
		})
	{
	}
}
