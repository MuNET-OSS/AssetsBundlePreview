using System.Collections.Generic;

namespace SpirV;

public class OpTypeVoid : Instruction
{
	public OpTypeVoid()
		: base("OpTypeVoid", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
