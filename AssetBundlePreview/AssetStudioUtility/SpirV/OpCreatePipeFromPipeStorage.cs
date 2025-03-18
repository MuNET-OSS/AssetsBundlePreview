using System.Collections.Generic;

namespace SpirV;

public class OpCreatePipeFromPipeStorage : Instruction
{
	public OpCreatePipeFromPipeStorage()
		: base("OpCreatePipeFromPipeStorage", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pipe Storage", OperandQuantifier.Default)
		})
	{
	}
}
