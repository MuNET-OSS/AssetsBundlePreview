using System.Collections.Generic;

namespace SpirV;

public class OpDecorationGroup : Instruction
{
	public OpDecorationGroup()
		: base("OpDecorationGroup", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
