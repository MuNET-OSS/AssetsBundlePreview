using System.Collections.Generic;

namespace SpirV;

public class OpConvertFToS : Instruction
{
	public OpConvertFToS()
		: base("OpConvertFToS", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Float Value", OperandQuantifier.Default)
		})
	{
	}
}
