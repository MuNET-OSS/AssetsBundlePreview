using System.Collections.Generic;

namespace SpirV;

public class LoopControlParameterFactory : ParameterFactory
{
	public class DependencyLengthParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public override Parameter CreateParameter(object value)
	{
		if ((LoopControl)value == LoopControl.DependencyLength)
		{
			return new DependencyLengthParameter();
		}
		return null;
	}
}
