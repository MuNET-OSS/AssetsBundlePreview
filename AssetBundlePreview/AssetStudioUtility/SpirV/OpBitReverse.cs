using System.Collections.Generic;

namespace SpirV;

public class OpBitReverse : Instruction
{
	public OpBitReverse()
		: base("OpBitReverse", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default)
		})
	{
	}
}
