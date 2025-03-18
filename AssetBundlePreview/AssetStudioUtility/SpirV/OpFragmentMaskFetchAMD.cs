using System.Collections.Generic;

namespace SpirV;

public class OpFragmentMaskFetchAMD : Instruction
{
	public OpFragmentMaskFetchAMD()
		: base("OpFragmentMaskFetchAMD", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default)
		})
	{
	}
}
