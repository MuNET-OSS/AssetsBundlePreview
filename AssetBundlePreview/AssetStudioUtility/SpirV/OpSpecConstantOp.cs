using System.Collections.Generic;

namespace SpirV;

public class OpSpecConstantOp : Instruction
{
	public OpSpecConstantOp()
		: base("OpSpecConstantOp", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralSpecConstantOpInteger(), "Opcode", OperandQuantifier.Default)
		})
	{
	}
}
