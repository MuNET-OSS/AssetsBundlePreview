using System.Collections.Generic;

namespace SpirV;

public class OpImageSparseSampleImplicitLod : Instruction
{
	public OpImageSparseSampleImplicitLod()
		: base("OpImageSparseSampleImplicitLod", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampled Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new EnumType<ImageOperands, ImageOperandsParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
