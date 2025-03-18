using System.Collections.Generic;

namespace SpirV;

public class OpExecutionMode : Instruction
{
	public OpExecutionMode()
		: base("OpExecutionMode", new List<Operand>
		{
			new Operand(new IdRef(), "Entry Point", OperandQuantifier.Default),
			new Operand(new EnumType<ExecutionMode, ExecutionModeParameterFactory>(), "Mode", OperandQuantifier.Default)
		})
	{
	}
}
