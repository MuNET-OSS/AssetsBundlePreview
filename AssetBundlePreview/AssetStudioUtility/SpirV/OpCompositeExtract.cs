using System.Collections.Generic;

namespace SpirV;

public class OpCompositeExtract : Instruction
{
	public OpCompositeExtract()
		: base("OpCompositeExtract", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Composite", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Indexes", OperandQuantifier.Varying)
		})
	{
	}
}
