using System.Collections.Generic;

namespace SpirV;

public class OpLine : Instruction
{
	public OpLine()
		: base("OpLine", new List<Operand>
		{
			new Operand(new IdRef(), "File", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Line", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Column", OperandQuantifier.Default)
		})
	{
	}
}
