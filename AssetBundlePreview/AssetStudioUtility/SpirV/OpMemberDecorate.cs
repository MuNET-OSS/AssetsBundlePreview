using System.Collections.Generic;

namespace SpirV;

public class OpMemberDecorate : Instruction
{
	public OpMemberDecorate()
		: base("OpMemberDecorate", new List<Operand>
		{
			new Operand(new IdRef(), "Structure Type", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Member", OperandQuantifier.Default),
			new Operand(new EnumType<Decoration, DecorationParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
