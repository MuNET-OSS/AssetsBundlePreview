using System.Collections.Generic;

namespace SpirV;

public class OpBitFieldInsert : Instruction
{
	public OpBitFieldInsert()
		: base("OpBitFieldInsert", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Insert", OperandQuantifier.Default),
			new Operand(new IdRef(), "Offset", OperandQuantifier.Default),
			new Operand(new IdRef(), "Count", OperandQuantifier.Default)
		})
	{
	}
}
