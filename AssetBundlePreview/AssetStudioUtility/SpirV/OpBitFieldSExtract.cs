using System.Collections.Generic;

namespace SpirV;

public class OpBitFieldSExtract : Instruction
{
	public OpBitFieldSExtract()
		: base("OpBitFieldSExtract", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Base", OperandQuantifier.Default),
			new Operand(new IdRef(), "Offset", OperandQuantifier.Default),
			new Operand(new IdRef(), "Count", OperandQuantifier.Default)
		})
	{
	}
}
