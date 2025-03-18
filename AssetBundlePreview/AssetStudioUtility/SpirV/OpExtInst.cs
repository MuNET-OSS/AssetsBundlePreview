using System.Collections.Generic;

namespace SpirV;

public class OpExtInst : Instruction
{
	public OpExtInst()
		: base("OpExtInst", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Set", OperandQuantifier.Default),
			new Operand(new LiteralExtInstInteger(), "Instruction", OperandQuantifier.Default),
			new Operand(new IdRef(), "Operand 1, +Operand 2, +...", OperandQuantifier.Varying)
		})
	{
	}
}
