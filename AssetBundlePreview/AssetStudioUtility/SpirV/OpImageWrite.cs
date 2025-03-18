using System.Collections.Generic;

namespace SpirV;

public class OpImageWrite : Instruction
{
	public OpImageWrite()
		: base("OpImageWrite", new List<Operand>
		{
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new IdRef(), "Texel", OperandQuantifier.Default),
			new Operand(new EnumType<ImageOperands, ImageOperandsParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
