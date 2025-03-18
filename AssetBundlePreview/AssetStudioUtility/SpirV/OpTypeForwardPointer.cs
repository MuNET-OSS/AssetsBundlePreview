using System.Collections.Generic;

namespace SpirV;

public class OpTypeForwardPointer : Instruction
{
	public OpTypeForwardPointer()
		: base("OpTypeForwardPointer", new List<Operand>
		{
			new Operand(new IdRef(), "Pointer Type", OperandQuantifier.Default),
			new Operand(new EnumType<StorageClass, StorageClassParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
