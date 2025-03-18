using System.Collections.Generic;

namespace SpirV;

public class OpImageTexelPointer : Instruction
{
	public OpImageTexelPointer()
		: base("OpImageTexelPointer", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Image", OperandQuantifier.Default),
			new Operand(new IdRef(), "Coordinate", OperandQuantifier.Default),
			new Operand(new IdRef(), "Sample", OperandQuantifier.Default)
		})
	{
	}
}
