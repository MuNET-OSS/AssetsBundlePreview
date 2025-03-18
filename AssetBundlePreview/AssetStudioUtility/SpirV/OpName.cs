using System.Collections.Generic;

namespace SpirV;

public class OpName : Instruction
{
	public OpName()
		: base("OpName", new List<Operand>
		{
			new Operand(new IdRef(), "Target", OperandQuantifier.Default),
			new Operand(new LiteralString(), "Name", OperandQuantifier.Default)
		})
	{
	}
}
