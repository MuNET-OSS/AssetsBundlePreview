using System.Collections.Generic;

namespace SpirV;

public class OpTypeBool : Instruction
{
	public OpTypeBool()
		: base("OpTypeBool", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
