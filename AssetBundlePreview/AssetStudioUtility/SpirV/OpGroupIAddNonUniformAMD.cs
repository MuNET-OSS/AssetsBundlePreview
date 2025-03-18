using System.Collections.Generic;

namespace SpirV;

public class OpGroupIAddNonUniformAMD : Instruction
{
	public OpGroupIAddNonUniformAMD()
		: base("OpGroupIAddNonUniformAMD", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdScope(), "Execution", OperandQuantifier.Default),
			new Operand(new EnumType<GroupOperation, GroupOperationParameterFactory>(), "Operation", OperandQuantifier.Default),
			new Operand(new IdRef(), "X", OperandQuantifier.Default)
		})
	{
	}
}
