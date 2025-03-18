using System.Collections.Generic;

namespace SpirV;

public class OpString : Instruction
{
	public OpString()
		: base("OpString", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralString(), "String", OperandQuantifier.Default)
		})
	{
	}
}
