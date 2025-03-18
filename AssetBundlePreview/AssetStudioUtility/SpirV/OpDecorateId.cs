using System.Collections.Generic;

namespace SpirV;

public class OpDecorateId : Instruction
{
	public OpDecorateId()
		: base("OpDecorateId", new List<Operand>
		{
			new Operand(new IdRef(), "Target", OperandQuantifier.Default),
			new Operand(new EnumType<Decoration, DecorationParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
