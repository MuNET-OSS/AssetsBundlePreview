using System.Collections.Generic;

namespace SpirV;

public class OpCaptureEventProfilingInfo : Instruction
{
	public OpCaptureEventProfilingInfo()
		: base("OpCaptureEventProfilingInfo", new List<Operand>
		{
			new Operand(new IdRef(), "Event", OperandQuantifier.Default),
			new Operand(new IdRef(), "Profiling Info", OperandQuantifier.Default),
			new Operand(new IdRef(), "Value", OperandQuantifier.Default)
		})
	{
	}
}
