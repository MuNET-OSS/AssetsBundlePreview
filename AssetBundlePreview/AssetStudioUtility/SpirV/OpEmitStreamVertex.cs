using System.Collections.Generic;

namespace SpirV;

public class OpEmitStreamVertex : Instruction
{
	public OpEmitStreamVertex()
		: base("OpEmitStreamVertex", new List<Operand>
		{
			new Operand(new IdRef(), "Stream", OperandQuantifier.Default)
		})
	{
	}
}
