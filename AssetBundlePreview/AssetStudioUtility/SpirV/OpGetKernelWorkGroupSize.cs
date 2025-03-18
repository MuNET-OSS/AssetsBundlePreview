using System.Collections.Generic;

namespace SpirV;

public class OpGetKernelWorkGroupSize : Instruction
{
	public OpGetKernelWorkGroupSize()
		: base("OpGetKernelWorkGroupSize", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Invoke", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Align", OperandQuantifier.Default)
		})
	{
	}
}
