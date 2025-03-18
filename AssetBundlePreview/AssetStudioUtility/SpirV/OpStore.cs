using System.Collections.Generic;

namespace SpirV;

public class OpStore : Instruction
{
	public OpStore()
		: base("OpStore", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new IdRef(), "Object", OperandQuantifier.Default),
			new Operand(new EnumType<MemoryAccess, MemoryAccessParameterFactory>(), null, OperandQuantifier.Optional)
		})
	{
	}
}
