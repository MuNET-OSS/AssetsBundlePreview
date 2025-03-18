using System.Collections.Generic;

namespace SpirV;

public class OpEndStreamPrimitive : Instruction
{
	public OpEndStreamPrimitive()
		: base("OpEndStreamPrimitive", new List<Operand>
		{
			new Operand(new IdRef(), "Stream", OperandQuantifier.Default)
		})
	{
	}
}
