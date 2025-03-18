using System.Collections.Generic;

namespace SpirV;

public class OpEntryPoint : Instruction
{
	public OpEntryPoint()
		: base("OpEntryPoint", new List<Operand>
		{
			new Operand(new EnumType<ExecutionModel, ExecutionModelParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new IdRef(), "Entry Point", OperandQuantifier.Default),
			new Operand(new LiteralString(), "Name", OperandQuantifier.Default),
			new Operand(new IdRef(), "Interface", OperandQuantifier.Varying)
		})
	{
	}
}
