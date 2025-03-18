using System.Collections.Generic;

namespace SpirV;

public class OpImageSparseTexelsResident : Instruction
{
	public OpImageSparseTexelsResident()
		: base("OpImageSparseTexelsResident", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Resident Code", OperandQuantifier.Default)
		})
	{
	}
}
