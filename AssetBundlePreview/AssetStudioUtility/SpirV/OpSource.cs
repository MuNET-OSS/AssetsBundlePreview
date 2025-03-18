using System.Collections.Generic;

namespace SpirV;

public class OpSource : Instruction
{
	public OpSource()
		: base("OpSource", new List<Operand>
		{
			new Operand(new EnumType<SourceLanguage, SourceLanguageParameterFactory>(), null, OperandQuantifier.Default),
			new Operand(new LiteralInteger(), "Version", OperandQuantifier.Default),
			new Operand(new IdRef(), "File", OperandQuantifier.Optional),
			new Operand(new LiteralString(), "Source", OperandQuantifier.Optional)
		})
	{
	}
}
