using System.Collections.Generic;

namespace SpirV;

public class OpTypeArray : Instruction
{
	public OpTypeArray()
		: base("OpTypeArray", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Element Type", OperandQuantifier.Default),
			new Operand(new IdRef(), "Length", OperandQuantifier.Default)
		})
	{
	}
}
