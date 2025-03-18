using System.Collections.Generic;

namespace SpirV;

public class OpSubgroupFirstInvocationKHR : Instruction
{
	public OpSubgroupFirstInvocationKHR()
		: base("OpSubgroupFirstInvocationKHR", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
