using System.Collections.Generic;

namespace SpirV;

public class OpMatrixTimesScalar : Instruction
{
	public OpMatrixTimesScalar()
		: base("OpMatrixTimesScalar", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Matrix", OperandQuantifier.Default),
			new Operand(new IdRef(), "Scalar", OperandQuantifier.Default)
		})
	{
	}
}
