using System.Collections.Generic;

namespace SpirV;

public class OpCommitWritePipe : Instruction
{
	public OpCommitWritePipe()
		: base("OpCommitWritePipe", new List<Operand>
		{
			new Operand(new IdRef(), "Pipe", OperandQuantifier.Default),
			new Operand(new IdRef(), "Reserve Id", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Alignment", OperandQuantifier.Default)
		})
	{
	}
}
