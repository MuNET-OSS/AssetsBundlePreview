using System.Collections.Generic;

namespace SpirV;

public class OpCopyMemory : Instruction
{
	public OpCopyMemory()
		: base("OpCopyMemory", new List<Operand>
		{
			new Operand(new IdRef(), "Target", OperandQuantifier.Default),
			new Operand(new IdRef(), "Source", OperandQuantifier.Default),
			new Operand(new EnumType<MemoryAccess, MemoryAccessParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
