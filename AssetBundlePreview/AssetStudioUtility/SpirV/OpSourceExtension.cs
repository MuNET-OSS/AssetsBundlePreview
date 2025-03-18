using System.Collections.Generic;

namespace SpirV;

public class OpSourceExtension : Instruction
{
	public OpSourceExtension()
		: base("OpSourceExtension", new List<Operand>
		{
			new Operand(new LiteralString(), "Extension", OperandQuantifier.Default)
		})
	{
	}
}
