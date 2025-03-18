using System.Collections.Generic;

namespace SpirV;

public class OpGenericCastToPtrExplicit : Instruction
{
	public OpGenericCastToPtrExplicit()
		: base("OpGenericCastToPtrExplicit", new List<Operand>
		{
			new Operand(new IdResultType(), null, OperandQuantifier.Default),
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Pointer", OperandQuantifier.Default),
			new Operand(new EnumType<StorageClass, StorageClassParameterFactory>(), "Storage", OperandQuantifier.Default)
		})
	{
	}
}
