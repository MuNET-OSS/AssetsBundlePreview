using System.Collections.Generic;

namespace SpirV;

public class OpReservedWritePipe : Instruction
{
	public OpReservedWritePipe()
		: base("OpReservedWritePipe", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pipe", OperandQuantifier.Default),
			new Operand(new IdRef(), "Reserve Id", OperandQuantifier.Default),
			new Operand(new IdRef(), "Index", OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Size", OperandQuantifier.Default),
			new Operand(new IdRef(), "Packet Alignment", OperandQuantifier.Default)
		})
	{
	}
}
