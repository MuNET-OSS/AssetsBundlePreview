using System.Collections.Generic;

namespace SpirV;

public class OpImageSparseSampleDrefImplicitLod : Instruction
{
	public OpImageSparseSampleDrefImplicitLod()
		: base("OpImageSparseSampleDrefImplicitLod", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampled Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new IdRef(), "D~ref~", OperandQuantifier.Default),
			new Operand(new EnumType<ImageOperands, ImageOperandsParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
