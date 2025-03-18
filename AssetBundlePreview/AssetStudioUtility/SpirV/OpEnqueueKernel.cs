using System.Collections.Generic;

namespace SpirV;

public class OpEnqueueKernel : Instruction
{
	public OpEnqueueKernel()
		: base("OpEnqueueKernel", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Queue", OperandQuantifier.Default),
			new Operand(new IdRef(), "Flags", OperandQuantifier.Default),
			new Operand(new IdRef(), "ND Range", OperandQuantifier.Default),
			new Operand(new IdRef(), "Num Events", OperandQuantifier.Default),
			new Operand(new IdRef(), "Wait Events", OperandQuantifier.Default),
			new Operand(new IdRef(), "Ret Event", OperandQuantifier.Default),
			new Operand(new IdRef(), "Invoke", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Align", OperandQuantifier.Default),
			new Operand(new IdRef(), "Local Size", OperandQuantifier.Varying)
		})
	{
	}
}
