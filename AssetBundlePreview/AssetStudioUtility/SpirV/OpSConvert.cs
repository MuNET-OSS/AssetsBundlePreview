using System.Collections.Generic;

namespace SpirV;

public class OpSConvert : Instruction
{
	public OpSConvert()
		: base("OpSConvert", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Signed Value", OperandQuantifier.Default)
		})
	{
	}
}
