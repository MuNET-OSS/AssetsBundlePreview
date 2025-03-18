using System.Collections.Generic;

namespace SpirV;

public class OpFunctionParameter : Instruction
{
	public OpFunctionParameter()
		: base("OpFunctionParameter", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
