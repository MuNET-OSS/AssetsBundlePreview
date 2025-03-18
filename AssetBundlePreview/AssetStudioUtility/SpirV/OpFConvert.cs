using System.Collections.Generic;

namespace SpirV;

public class OpFConvert : Instruction
{
	public OpFConvert()
		: base("OpFConvert", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Float Value", OperandQuantifier.Default)
		})
	{
	}
}
