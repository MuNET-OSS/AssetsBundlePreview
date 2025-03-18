using System.Collections.Generic;

namespace SpirV;

public class OpConstantSampler : Instruction
{
	public OpConstantSampler()
		: base("OpConstantSampler", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new EnumType<SamplerAddressingMode, SamplerAddressingModeParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Param", OperandQuantifier.Default),
			new Operand(new EnumType<SamplerFilterMode, SamplerFilterModeParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
