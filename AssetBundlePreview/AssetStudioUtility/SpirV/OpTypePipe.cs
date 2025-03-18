using System.Collections.Generic;

namespace SpirV;

public class OpTypePipe : Instruction
{
	public OpTypePipe()
		: base("OpTypePipe", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default),
			new Operand(new EnumType<AccessQualifier, AccessQualifierParameterFactory>(), "Qualifier", OperandQuantifier.Default)
		})
	{
	}
}
