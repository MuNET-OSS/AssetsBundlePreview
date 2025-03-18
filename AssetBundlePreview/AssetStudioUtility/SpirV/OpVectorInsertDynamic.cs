using System.Collections.Generic;

namespace SpirV;

public class OpVectorInsertDynamic : Instruction
{
	public OpVectorInsertDynamic()
		: base("OpVectorInsertDynamic", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Vector", OperandQuantifier.Default),
			new Operand(new IdRef(), "Component", OperandQuantifier.Default),
			new Operand(new IdRef(), "Index", OperandQuantifier.Default)
		})
	{
	}
}
