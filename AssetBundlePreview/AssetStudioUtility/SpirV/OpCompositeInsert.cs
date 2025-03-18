using System.Collections.Generic;

namespace SpirV;

public class OpCompositeInsert : Instruction
{
	public OpCompositeInsert()
		: base("OpCompositeInsert", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Object", OperandQuantifier.Default),
			new Operand(new IdRef(), "Composite", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Indexes", OperandQuantifier.Varying)
		})
	{
	}
}
