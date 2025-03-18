using System.Collections.Generic;

namespace SpirV;

public class ExecutionModeParameterFactory : ParameterFactory
{
	public class InvocationsParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LocalSizeParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger(),
			new LiteralInteger(),
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LocalSizeHintParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger(),
			new LiteralInteger(),
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class OutputVerticesParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class VecTypeHintParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class SubgroupSizeParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class SubgroupsPerWorkgroupParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class SubgroupsPerWorkgroupIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LocalSizeIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef(),
			new IdRef(),
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LocalSizeHintIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public override Parameter CreateParameter(object value)
	{
		return (ExecutionMode)value switch
		{
			ExecutionMode.Invocations => new InvocationsParameter(), 
			ExecutionMode.LocalSize => new LocalSizeParameter(), 
			ExecutionMode.LocalSizeHint => new LocalSizeHintParameter(), 
			ExecutionMode.OutputVertices => new OutputVerticesParameter(), 
			ExecutionMode.VecTypeHint => new VecTypeHintParameter(), 
			ExecutionMode.SubgroupSize => new SubgroupSizeParameter(), 
			ExecutionMode.SubgroupsPerWorkgroup => new SubgroupsPerWorkgroupParameter(), 
			ExecutionMode.SubgroupsPerWorkgroupId => new SubgroupsPerWorkgroupIdParameter(), 
			ExecutionMode.LocalSizeId => new LocalSizeIdParameter(), 
			ExecutionMode.LocalSizeHintId => new LocalSizeHintIdParameter(), 
			_ => null, 
		};
	}
}
