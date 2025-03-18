using System.Collections.Generic;

namespace SpirV;

public class OpShiftRightLogical : Instruction
{
	public OpShiftRightLogical()
		: base("OpShiftRightLogical", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Shift", OperandQuantifier.Default)
		})
	{
	}
}
