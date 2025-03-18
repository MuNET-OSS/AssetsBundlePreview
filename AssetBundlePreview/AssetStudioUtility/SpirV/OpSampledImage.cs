using System.Collections.Generic;

namespace SpirV;

public class OpSampledImage : Instruction
{
	public OpSampledImage()
		: base("OpSampledImage", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampler", OperandQuantifier.Default)
		})
	{
	}
}
