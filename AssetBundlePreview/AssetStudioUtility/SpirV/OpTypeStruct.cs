using System.Collections.Generic;

namespace SpirV;

public class OpTypeStruct : Instruction
{
	public OpTypeStruct()
		: base("OpTypeStruct", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Member 0 type, +member 1 type, +...", OperandQuantifier.Varying)
		})
	{
	}
}
