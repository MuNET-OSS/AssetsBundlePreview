using System.Collections.Generic;

namespace SpirV;

public class OpFunction : Instruction
{
	public OpFunction()
		: base("OpFunction", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new EnumType<FunctionControl, FunctionControlParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Function Type", OperandQuantifier.Default)
		})
	{
	}
}
