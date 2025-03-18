using System.Collections.Generic;

namespace SpirV;

public class OpReturnValue : Instruction
{
	public OpReturnValue()
		: base("OpReturnValue", new List<Operand>
		{
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
