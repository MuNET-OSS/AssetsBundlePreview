using System.Collections.Generic;

namespace SpirV;

public class OpNamedBarrierInitialize : Instruction
{
	public OpNamedBarrierInitialize()
		: base("OpNamedBarrierInitialize", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Subgroup Count", OperandQuantifier.Default)
		})
	{
	}
}
