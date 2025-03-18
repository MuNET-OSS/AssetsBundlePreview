using System.Collections.Generic;

namespace SpirV;

public class ImageOperandsParameterFactory : ParameterFactory
{
	public class BiasParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LodParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class GradParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef(),
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class ConstOffsetParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class OffsetParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class ConstOffsetsParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class SampleParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class MinLodParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public override Parameter CreateParameter(object value)
	{
		return (ImageOperands)value switch
		{
			ImageOperands.Bias => new BiasParameter(), 
			ImageOperands.Lod => new LodParameter(), 
			ImageOperands.Grad => new GradParameter(), 
			ImageOperands.ConstOffset => new ConstOffsetParameter(), 
			ImageOperands.Offset => new OffsetParameter(), 
			ImageOperands.ConstOffsets => new ConstOffsetsParameter(), 
			ImageOperands.Sample => new SampleParameter(), 
			ImageOperands.MinLod => new MinLodParameter(), 
			_ => null, 
		};
	}
}
