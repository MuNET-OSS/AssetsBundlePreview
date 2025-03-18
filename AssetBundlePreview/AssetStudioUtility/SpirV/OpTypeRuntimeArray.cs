using System.Collections.Generic;

namespace SpirV;

public class OpTypeRuntimeArray : Instruction
{
	public OpTypeRuntimeArray()
		: base("OpTypeRuntimeArray", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Element Type", OperandQuantifier.Default)
		})
	{
	}
}
