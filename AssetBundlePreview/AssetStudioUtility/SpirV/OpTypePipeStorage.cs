using System.Collections.Generic;

namespace SpirV;

public class OpTypePipeStorage : Instruction
{
	public OpTypePipeStorage()
		: base("OpTypePipeStorage", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
