using System.Collections.Generic;

namespace SpirV;

public class OpFwidthCoarse : Instruction
{
	public OpFwidthCoarse()
		: base("OpFwidthCoarse", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "P", OperandQuantifier.Default)
		})
	{
	}
}
