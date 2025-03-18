using System.Collections.Generic;

namespace SpirV;

public class OpVectorShuffle : Instruction
{
	public OpVectorShuffle()
		: base("OpVectorShuffle", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector 1", OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector 2", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Components", OperandQuantifier.Varying)
		})
	{
	}
}
