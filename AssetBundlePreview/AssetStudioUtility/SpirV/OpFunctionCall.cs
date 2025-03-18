using System.Collections.Generic;

namespace SpirV;

public class OpFunctionCall : Instruction
{
	public OpFunctionCall()
		: base("OpFunctionCall", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Function", OperandQuantifier.Default),
			new Operand(new IdRef(), "Argument 0, +Argument 1, +...", OperandQuantifier.Varying)
		})
	{
	}
}
