using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupReadInvocationKHR : Instruction
{
	public OpSubgroupReadInvocationKHR()
		: base("OpSubgroupReadInvocationKHR", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default),
			new Operand(new IdRef(), "Index", OperandQuantifier.Default)
		})
	{
	}
}
