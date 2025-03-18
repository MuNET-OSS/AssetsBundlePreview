using System.Collections.Generic;

namespace SpirV;

public class DecorationParameterFactory : ParameterFactory
{
	public class SpecIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class ArrayStrideParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class MatrixStrideParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class BuiltInParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new EnumType<BuiltIn>()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class StreamParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LocationParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class ComponentParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class IndexParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class BindingParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class DescriptorSetParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class OffsetParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class XfbBufferParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class XfbStrideParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class FuncParamAttrParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new EnumType<FunctionParameterAttribute>()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class FPRoundingModeParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new EnumType<FPRoundingMode>()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class FPFastMathModeParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new EnumType<FPFastMathMode>()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class LinkageAttributesParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralString(),
			new EnumType<LinkageType>()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class InputAttachmentIndexParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class AlignmentParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class MaxByteOffsetParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class AlignmentIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class MaxByteOffsetIdParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new IdRef()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public class SecondaryViewportRelativeNVParameter : Parameter
	{
		private static readonly List<OperandType> operandTypes_ = new List<OperandType>
		{
			new LiteralInteger()
		};

		public override IReadOnlyList<OperandType> OperandTypes => operandTypes_;
	}

	public override Parameter CreateParameter(object value)
	{
		return (Decoration)value switch
		{
			Decoration.SpecId => new SpecIdParameter(), 
			Decoration.ArrayStride => new ArrayStrideParameter(), 
			Decoration.MatrixStride => new MatrixStrideParameter(), 
			Decoration.BuiltIn => new BuiltInParameter(), 
			Decoration.Stream => new StreamParameter(), 
			Decoration.Location => new LocationParameter(), 
			Decoration.Component => new ComponentParameter(), 
			Decoration.Index => new IndexParameter(), 
			Decoration.Binding => new BindingParameter(), 
			Decoration.DescriptorSet => new DescriptorSetParameter(), 
			Decoration.Offset => new OffsetParameter(), 
			Decoration.XfbBuffer => new XfbBufferParameter(), 
			Decoration.XfbStride => new XfbStrideParameter(), 
			Decoration.FuncParamAttr => new FuncParamAttrParameter(), 
			Decoration.FPRoundingMode => new FPRoundingModeParameter(), 
			Decoration.FPFastMathMode => new FPFastMathModeParameter(), 
			Decoration.LinkageAttributes => new LinkageAttributesParameter(), 
			Decoration.InputAttachmentIndex => new InputAttachmentIndexParameter(), 
			Decoration.Alignment => new AlignmentParameter(), 
			Decoration.MaxByteOffset => new MaxByteOffsetParameter(), 
			Decoration.AlignmentId => new AlignmentIdParameter(), 
			Decoration.MaxByteOffsetId => new MaxByteOffsetIdParameter(), 
			Decoration.SecondaryViewportRelativeNV => new SecondaryViewportRelativeNVParameter(), 
			_ => null, 
		};
	}
}
