using System.Collections.Generic;

namespace SpirV;

public class OpBuildNDRange : Instruction
{
	public OpBuildNDRange()
		: base("OpBuildNDRange", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "GlobalWorkSize", OperandQuantifier.Default),
			new Operand(new IdRef(), "LocalWorkSize", OperandQuantifier.Default),
			new Operand(new IdRef(), "GlobalWorkOffset", OperandQuantifier.Default)
		})
	{
	}
}
