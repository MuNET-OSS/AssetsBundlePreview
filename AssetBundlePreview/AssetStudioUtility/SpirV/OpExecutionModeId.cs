using System.Collections.Generic;

namespace SpirV;

public class OpExecutionModeId : Instruction
{
	public OpExecutionModeId()
		: base("OpExecutionModeId", new List<Operand>
		{
			new Operand(new IdRef(), "Entry Point", OperandQuantifier.Default),
			new Operand(new EnumType<ExecutionMode, ExecutionModeParameterFactory>(), "Mode", OperandQuantifier.Default)
		})
	{
	}
}
