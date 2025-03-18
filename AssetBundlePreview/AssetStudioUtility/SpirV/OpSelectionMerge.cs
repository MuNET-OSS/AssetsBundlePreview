using System.Collections.Generic;

namespace SpirV;

public class OpSelectionMerge : Instruction
{
	public OpSelectionMerge()
		: base("OpSelectionMerge", new List<Operand>
		{
			new Operand(new IdRef(), "Merge Block", OperandQuantifier.Default),
			new Operand(new EnumType<SelectionControl, SelectionControlParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
