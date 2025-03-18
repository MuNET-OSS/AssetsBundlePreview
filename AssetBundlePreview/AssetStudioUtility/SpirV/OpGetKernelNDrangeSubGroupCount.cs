using System.Collections.Generic;

namespace SpirV;

public class OpGetKernelNDrangeSubGroupCount : Instruction
{
	public OpGetKernelNDrangeSubGroupCount()
		: base("OpGetKernelNDrangeSubGroupCount", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "ND Range", OperandQuantifier.Default),
			new Operand(new IdRef(), "Invoke", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Param Align", OperandQuantifier.Default)
		})
	{
	}
}
