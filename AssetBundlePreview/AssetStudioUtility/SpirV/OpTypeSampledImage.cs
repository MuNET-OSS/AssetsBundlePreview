using System.Collections.Generic;

namespace SpirV;

public class OpTypeSampledImage : Instruction
{
	public OpTypeSampledImage()
		: base("OpTypeSampledImage", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image Type", OperandQuantifier.Default)
		})
	{
	}
}
