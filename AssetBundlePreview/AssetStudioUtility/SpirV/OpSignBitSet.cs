using System.Collections.Generic;

namespace SpirV;

public class OpSignBitSet : Instruction
{
	public OpSignBitSet()
		: base("OpSignBitSet", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "x", OperandQuantifier.Default)
		})
	{
	}
}
