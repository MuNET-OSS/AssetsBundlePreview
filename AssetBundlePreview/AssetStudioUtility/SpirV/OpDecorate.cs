using System.Collections.Generic;

namespace SpirV;

public class OpDecorate : Instruction
{
	public OpDecorate()
		: base("OpDecorate", new List<Operand>
		{
			new Operand(new IdRef(), "Target", OperandQuantifier.Default),
			new Operand(new EnumType<Decoration, DecorationParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
