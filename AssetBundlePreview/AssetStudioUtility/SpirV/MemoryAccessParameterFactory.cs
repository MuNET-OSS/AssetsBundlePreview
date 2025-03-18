using System.Collections.Generic;

namespace SpirV;

public class MemoryAccessParameterFactory : ParameterFactory
{
	public class AlignedParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public override Parameter CreateParameter(object value)
	{
		if ((MemoryAccess)value == MemoryAccess.Aligned)
		{
			return new AlignedParameter();
		}
		return null;
	}
}
