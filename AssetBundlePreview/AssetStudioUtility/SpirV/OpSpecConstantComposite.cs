using System.Collections.Generic;

namespace SpirV;

public class OpSpecConstantComposite : Instruction
{
	public OpSpecConstantComposite()
		: base("OpSpecConstantComposite", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Constituents", OperandQuantifier.Varying)
		})
	{
	}
}
