using System.Collections.Generic;

namespace SpirV;

public class OpVariable : Instruction
{
	public OpVariable()
		: base("OpVariable", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new EnumType<StorageClass, StorageClassParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Initializer", OperandQuantifier.Optional)
		})
	{
	}
}
