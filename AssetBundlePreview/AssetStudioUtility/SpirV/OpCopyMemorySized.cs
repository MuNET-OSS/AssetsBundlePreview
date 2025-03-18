using System.Collections.Generic;

namespace SpirV;

public class OpCopyMemorySized : Instruction
{
	public OpCopyMemorySized()
		: base("OpCopyMemorySized", new List<Operand>
		{
			new Operand(new IdRef(), "Target", OperandQuantifier.Default),
			new Operand(new IdRef(), "Source", OperandQuantifier.Default),
			new Operand(new IdRef(), "Size", OperandQuantifier.Default),
			new Operand(new EnumType<MemoryAccess, MemoryAccessParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
