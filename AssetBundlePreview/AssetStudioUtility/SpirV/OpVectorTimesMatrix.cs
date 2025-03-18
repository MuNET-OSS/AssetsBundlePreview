using System.Collections.Generic;

namespace SpirV;

public class OpVectorTimesMatrix : Instruction
{
	public OpVectorTimesMatrix()
		: base("OpVectorTimesMatrix", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector", OperandQuantifier.Default),
			new Operand(new IdRef(), "Matrix", OperandQuantifier.Default)
		})
	{
	}
}
