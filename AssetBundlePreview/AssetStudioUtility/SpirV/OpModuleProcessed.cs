using System.Collections.Generic;

namespace SpirV;

public class OpModuleProcessed : Instruction
{
	public OpModuleProcessed()
		: base("OpModuleProcessed", new List<Operand>
		{
			new Operand(new LiteralString(), "Process", OperandQuantifier.Default)
		})
	{
	}
}
