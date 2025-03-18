using System.Collections.Generic;

namespace SpirV;

public class OpPhi : Instruction
{
	public OpPhi()
		: base("OpPhi", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new PairIdRefIdRef(), "Variable, Parent, ...", OperandQuantifier.Varying)
		})
	{
	}
}
