using System.Collections.Generic;

namespace SpirV;

public class OpFragmentFetchAMD : Instruction
{
	public OpFragmentFetchAMD()
		: base("OpFragmentFetchAMD", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new IdRef(), "Fragment Index", OperandQuantifier.Default)
		})
	{
	}
}
