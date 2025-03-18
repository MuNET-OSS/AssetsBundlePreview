using System.Collections.Generic;

namespace SpirV;

public class OpShiftRightArithmetic : Instruction
{
	public OpShiftRightArithmetic()
		: base("OpShiftRightArithmetic", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Shift", OperandQuantifier.Default)
		})
	{
	}
}
