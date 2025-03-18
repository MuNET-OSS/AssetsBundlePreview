using System.Collections.Generic;

namespace SpirV;

public class OpImageFetch : Instruction
{
	public OpImageFetch()
		: base("OpImageFetch", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new EnumType<ImageOperands, ImageOperandsParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
