using System.Collections.Generic;

namespace SpirV;

public class OpCapability : Instruction
{
	public OpCapability()
		: base("OpCapability", new List<Operand>
		{
			new Operand(new EnumType<Capability, CapabilityParameterFactory>(), "Capability", OperandQuantifier.Default)
		})
	{
	}
}
