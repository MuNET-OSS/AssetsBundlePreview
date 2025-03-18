using System.Collections.Generic;

namespace SpirV;

public class OpBranch : Instruction
{
	public OpBranch()
		: base("OpBranch", new List<Operand>
		{
			new Operand(new IdRef(), "Target Label", OperandQuantifier.Default)
		})
	{
	}
}
