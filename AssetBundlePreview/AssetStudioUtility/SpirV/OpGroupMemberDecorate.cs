using System.Collections.Generic;

namespace SpirV;

public class OpGroupMemberDecorate : Instruction
{
	public OpGroupMemberDecorate()
		: base("OpGroupMemberDecorate", new List<Operand>
		{
			new Operand(new IdRef(), "Decoration Group", OperandQuantifier.Default),
			new Operand(new PairIdRefLiteralInteger(), "Targets", OperandQuantifier.Varying)
		})
	{
	}
}
