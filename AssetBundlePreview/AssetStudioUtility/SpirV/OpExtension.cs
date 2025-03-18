using System.Collections.Generic;

namespace SpirV;

public class OpExtension : Instruction
{
	public OpExtension()
		: base("OpExtension", new List<Operand>
		{
			new Operand(new LiteralString(), "Name", OperandQuantifier.Default)
		})
	{
	}
}
