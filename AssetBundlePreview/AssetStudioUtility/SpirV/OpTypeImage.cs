using System.Collections.Generic;

namespace SpirV;

public class OpTypeImage : Instruction
{
	public OpTypeImage()
		: base("OpTypeImage", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Sampled Type", OperandQuantifier.Default),
			new Operand(new EnumType<Dim, DimParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Depth", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Arrayed", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "MS", OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Sampled", OperandQuantifier.Default),
			new Operand(new EnumType<ImageFormat, ImageFormatParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new EnumType<AccessQualifier, AccessQualifierParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
