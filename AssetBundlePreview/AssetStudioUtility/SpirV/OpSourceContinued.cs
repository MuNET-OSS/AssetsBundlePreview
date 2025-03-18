using System.Collections.Generic;

namespace SpirV;

public class OpSourceContinued : Instruction
{
	public OpSourceContinued()
		: base("OpSourceContinued", new List<Operand>
		{
			new Operand(new LiteralString(), "Continued Source", OperandQuantifier.Default)
		})
	{
	}
}
