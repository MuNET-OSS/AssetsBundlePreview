using System.Collections.Generic;

namespace SpirV;

public class OpConstantPipeStorage : Instruction
{
	public OpConstantPipeStorage()
		: base("OpConstantPipeStorage", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Packet Size", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Packet Alignment", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Capacity", OperandQuantifier.Default)
		})
	{
	}
}
