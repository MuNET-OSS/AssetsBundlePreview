using System.Collections.Generic;

namespace SpirV;

public class OpReserveReadPipePackets : Instruction
{
	public OpReserveReadPipePackets()
		: base("OpReserveReadPipePackets", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pipe", OperandQuantifier.Default),
			new Operand(new IdRef(), "Num Packets", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Alignment", OperandQuantifier.Default)
		})
	{
	}
}
