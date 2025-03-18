using System.Collections.Generic;

namespace SpirV;

public class OpBranchConditional : Instruction
{
	public OpBranchConditional()
		: base("OpBranchConditional", new List<Operand>
		{
			new Operand(new IdRef(), "Condition", OperandQuantifier.Default),
			new Operand(new IdRef(), "True Label", OperandQuantifier.Default),
			new Operand(new IdRef(), "False Label", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Branch weights", OperandQuantifier.Varying)
		})
	{
	}
}
