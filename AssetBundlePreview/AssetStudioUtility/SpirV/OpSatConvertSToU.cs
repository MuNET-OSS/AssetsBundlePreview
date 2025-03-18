using System.Collections.Generic;

namespace SpirV;

public class OpSatConvertSToU : Instruction
{
	public OpSatConvertSToU()
		: base("OpSatConvertSToU", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Signed Value", OperandQuantifier.Default)
		})
	{
	}
}
