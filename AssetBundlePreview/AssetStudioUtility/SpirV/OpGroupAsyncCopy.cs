using System.Collections.Generic;

namespace SpirV;

public class OpGroupAsyncCopy : Instruction
{
	public OpGroupAsyncCopy()
		: base("OpGroupAsyncCopy", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new IdRef(), "Destination", OperandQuantifier.Default),
			new Operand(new IdRef(), "Source", OperandQuantifier.Default),
			new Operand(new IdRef(), "Num Elements", OperandQuantifier.Default),
			new Operand(new IdRef(), "Stride", OperandQuantifier.Default),
			new Operand(new IdRef(), "Event", OperandQuantifier.Default)
		})
	{
	}
}
