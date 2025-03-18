using System.Collections.Generic;

namespace SpirV;

public class OpSelect : Instruction
{
	public OpSelect()
		: base("OpSelect", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Condition", OperandQuantifier.Default),
			new Operand(new IdRef(), "Object 1", OperandQuantifier.Default),
			new Operand(new IdRef(), "Object 2", OperandQuantifier.Default)
		})
	{
	}
}
