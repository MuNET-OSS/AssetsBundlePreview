using System.Collections.Generic;

namespace SpirV;

public class OpArrayLength : Instruction
{
	public OpArrayLength()
		: base("OpArrayLength", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Structure", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Array member", OperandQuantifier.Default)
		})
	{
	}
}
