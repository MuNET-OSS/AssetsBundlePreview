using System.Collections.Generic;

namespace SpirV;

public class OpTypePointer : Instruction
{
	public OpTypePointer()
		: base("OpTypePointer", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new EnumType<StorageClass, StorageClassParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Type", OperandQuantifier.Default)
		})
	{
	}
}
