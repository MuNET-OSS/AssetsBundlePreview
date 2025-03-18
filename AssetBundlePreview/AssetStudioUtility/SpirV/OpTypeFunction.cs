using System.Collections.Generic;

namespace SpirV;

public class OpTypeFunction : Instruction
{
	public OpTypeFunction()
		: base("OpTypeFunction", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Return Type", OperandQuantifier.Default),
			new Operand(new IdRef(), "Parameter 0 Type, +Parameter 1 Type, +...", OperandQuantifier.Varying)
		})
	{
	}
}
