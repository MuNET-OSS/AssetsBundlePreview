using System.Collections.Generic;

namespace SpirV;

public class OpExtInstImport : Instruction
{
	public OpExtInstImport()
		: base("OpExtInstImport", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new LiteralString(), "Name", OperandQuantifier.Default)
		})
	{
	}
}
