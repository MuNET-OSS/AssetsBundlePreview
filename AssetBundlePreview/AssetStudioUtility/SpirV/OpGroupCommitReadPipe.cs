using System.Collections.Generic;

namespace SpirV;

public class OpGroupCommitReadPipe : Instruction
{
	public OpGroupCommitReadPipe()
		: base("OpGroupCommitReadPipe", new List<Operand>
		{
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdRef(), "Pipe", OperandQuantifier.Default),
			new Operand(new IdRef(), "Reserve Id", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Alignment", OperandQuantifier.Default)
		})
	{
	}
}
