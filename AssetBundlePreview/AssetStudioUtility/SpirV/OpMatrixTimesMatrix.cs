using System.Collections.Generic;

namespace SpirV;

public class OpMatrixTimesMatrix : Instruction
{
	public OpMatrixTimesMatrix()
		: base("OpMatrixTimesMatrix", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "LeftMatrix", OperandQuantifier.Default),
			new Operand(new IdRef(), "RightMatrix", OperandQuantifier.Default)
		})
	{
	}
}
