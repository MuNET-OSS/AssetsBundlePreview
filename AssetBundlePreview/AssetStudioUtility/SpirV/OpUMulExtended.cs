using System.Collections.Generic;

namespace SpirV;

public class OpUMulExtended : Instruction
{
	public OpUMulExtended()
		: base("OpUMulExtended", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand 1", OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand 2", OperandQuantifier.Default)
		})
	{
	}
}
