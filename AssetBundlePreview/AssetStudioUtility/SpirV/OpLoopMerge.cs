using System.Collections.Generic;

namespace SpirV;

public class OpLoopMerge : Instruction
{
	public OpLoopMerge()
		: base("OpLoopMerge", new List<Operand>
		{
			new Operand(new IdRef(), "Merge Block", OperandQuantifier.Default),
			new Operand(new IdRef(), "Continue Target", OperandQuantifier.Default),
			new Operand(new EnumType<LoopControl, LoopControlParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
