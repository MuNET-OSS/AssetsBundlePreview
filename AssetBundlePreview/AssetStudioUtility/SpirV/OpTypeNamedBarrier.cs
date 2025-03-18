using System.Collections.Generic;

namespace SpirV;

public class OpTypeNamedBarrier : Instruction
{
	public OpTypeNamedBarrier()
		: base("OpTypeNamedBarrier", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
