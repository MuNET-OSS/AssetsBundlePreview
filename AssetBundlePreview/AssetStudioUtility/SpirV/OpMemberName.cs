using System.Collections.Generic;

namespace SpirV;

public class OpMemberName : Instruction
{
	public OpMemberName()
		: base("OpMemberName", new List<Operand>
		{
			new Operand(new IdRef(), "Type", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Member", OperandQuantifier.Default),
			new Operand(new LiteralString(), "Name", OperandQuantifier.Default)
		})
	{
	}
}
