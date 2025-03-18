using System.Collections.Generic;

namespace SpirV;

public class OpLoad : Instruction
{
	public OpLoad()
		: base("OpLoad", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new EnumType<MemoryAccess, MemoryAccessParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
