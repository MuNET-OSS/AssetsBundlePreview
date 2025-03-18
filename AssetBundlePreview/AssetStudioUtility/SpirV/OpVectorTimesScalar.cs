using System.Collections.Generic;

namespace SpirV;

public class OpVectorTimesScalar : Instruction
{
	public OpVectorTimesScalar()
		: base("OpVectorTimesScalar", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector", OperandQuantifier.Default),
			new Operand(new IdRef(), "Scalar", OperandQuantifier.Default)
		})
	{
	}
}
