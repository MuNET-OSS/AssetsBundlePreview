using System.Collections.Generic;

namespace SpirV;

public class OpSwitch : Instruction
{
	public OpSwitch()
		: base("OpSwitch", new List<Operand>
		{
			new Operand(new IdRef(), "Selector", OperandQuantifier.Default),
			new Operand(new IdRef(), "Default", OperandQuantifier.Default),
			new Operand(new PairLiteralIntegerIdRef(), "Target", OperandQuantifier.Varying)
		})
	{
	}
}
