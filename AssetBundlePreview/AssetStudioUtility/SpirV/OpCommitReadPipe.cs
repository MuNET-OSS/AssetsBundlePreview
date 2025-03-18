using System.Collections.Generic;

namespace SpirV;

public class OpCommitReadPipe : Instruction
{
	public OpCommitReadPipe()
		: base("OpCommitReadPipe", new List<Operand>
		{
			new Operand(new IdRef(), "Pipe", OperandQuantifier.Default),
			new Operand(new IdRef(), "Reserve Id", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Alignment", OperandQuantifier.Default)
		})
	{
	}
}
