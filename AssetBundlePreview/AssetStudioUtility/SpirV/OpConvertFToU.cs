using System.Collections.Generic;

namespace SpirV;

public class OpConvertFToU : Instruction
{
	public OpConvertFToU()
		: base("OpConvertFToU", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Float Value", OperandQuantifier.Default)
		})
	{
	}
}
