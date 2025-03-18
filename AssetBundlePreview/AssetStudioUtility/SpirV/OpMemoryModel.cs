using System.Collections.Generic;

namespace SpirV;

public class OpMemoryModel : Instruction
{
	public OpMemoryModel()
		: base("OpMemoryModel", new List<Operand>
		{
			new Operand(new EnumType<AddressingModel, AddressingModelParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new EnumType<MemoryModel, MemoryModelParameterFactory>(), null, OperandQuantifier.Default)
		})
	{
	}
}
