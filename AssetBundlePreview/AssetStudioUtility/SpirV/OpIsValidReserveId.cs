using System.Collections.Generic;

namespace SpirV;

public class OpIsValidReserveId : Instruction
{
	public OpIsValidReserveId()
		: base("OpIsValidReserveId", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Reserve Id", OperandQuantifier.Default)
		})
	{
	}
}
