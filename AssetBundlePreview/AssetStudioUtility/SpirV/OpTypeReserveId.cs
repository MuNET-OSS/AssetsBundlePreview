using System.Collections.Generic;

namespace SpirV;

public class OpTypeReserveId : Instruction
{
	public OpTypeReserveId()
		: base("OpTypeReserveId", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
