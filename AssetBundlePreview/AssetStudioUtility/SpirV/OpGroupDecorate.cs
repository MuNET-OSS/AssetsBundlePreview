using System.Collections.Generic;

namespace SpirV;

public class OpGroupDecorate : Instruction
{
	public OpGroupDecorate()
		: base("OpGroupDecorate", new List<Operand>
		{
			new Operand(new IdRef(), "Decoration Group", OperandQuantifier.Default),
			new Operand(new IdRef(), "Targets", OperandQuantifier.Varying)
		})
	{
	}
}
