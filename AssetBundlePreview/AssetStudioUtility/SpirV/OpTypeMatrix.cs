using System.Collections.Generic;

namespace SpirV;

public class OpTypeMatrix : Instruction
{
	public OpTypeMatrix()
		: base("OpTypeMatrix", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Column Type", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Column Count", OperandQuantifier.Default)
		})
	{
	}
}
