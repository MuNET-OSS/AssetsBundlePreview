using System.Collections.Generic;

namespace SpirV;

public class OpTypeDeviceEvent : Instruction
{
	public OpTypeDeviceEvent()
		: base("OpTypeDeviceEvent", new List<Operand>
		{
			new Operand(new IdResult(), null, OperandQuantifier.Default)
		})
	{
	}
}
