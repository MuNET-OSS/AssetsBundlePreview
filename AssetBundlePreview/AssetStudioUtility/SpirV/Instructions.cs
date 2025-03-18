using System.Collections.Generic;

namespace SpirV;

public static class Instructions
{
	private static readonly Dictionary<int, Instruction> instructions_ = new Dictionary<int, Instruction>
	{
		{
			0,
			new OpNop()
		},
		{
			1,
			new OpUndef()
		},
		{
			2,
			new OpSourceContinued()
		},
		{
			3,
			new OpSource()
		},
		{
			4,
			new OpSourceExtension()
		},
		{
			5,
			new OpName()
		},
		{
			6,
			new OpMemberName()
		},
		{
			7,
			new OpString()
		},
		{
			8,
			new OpLine()
		},
		{
			10,
			new OpExtension()
		},
		{
			11,
			new OpExtInstImport()
		},
		{
			12,
			new OpExtInst()
		},
		{
			14,
			new OpMemoryModel()
		},
		{
			15,
			new OpEntryPoint()
		},
		{
			16,
			new OpExecutionMode()
		},
		{
			17,
			new OpCapability()
		},
		{
			19,
			new OpTypeVoid()
		},
		{
			20,
			new OpTypeBool()
		},
		{
			21,
			new OpTypeInt()
		},
		{
			22,
			new OpTypeFloat()
		},
		{
			23,
			new OpTypeVector()
		},
		{
			24,
			new OpTypeMatrix()
		},
		{
			25,
			new OpTypeImage()
		},
		{
			26,
			new OpTypeSampler()
		},
		{
			27,
			new OpTypeSampledImage()
		},
		{
			28,
			new OpTypeArray()
		},
		{
			29,
			new OpTypeRuntimeArray()
		},
		{
			30,
			new OpTypeStruct()
		},
		{
			31,
			new OpTypeOpaque()
		},
		{
			32,
			new OpTypePointer()
		},
		{
			33,
			new OpTypeFunction()
		},
		{
			34,
			new OpTypeEvent()
		},
		{
			35,
			new OpTypeDeviceEvent()
		},
		{
			36,
			new OpTypeReserveId()
		},
		{
			37,
			new OpTypeQueue()
		},
		{
			38,
			new OpTypePipe()
		},
		{
			39,
			new OpTypeForwardPointer()
		},
		{
			41,
			new OpConstantTrue()
		},
		{
			42,
			new OpConstantFalse()
		},
		{
			43,
			new OpConstant()
		},
		{
			44,
			new OpConstantComposite()
		},
		{
			45,
			new OpConstantSampler()
		},
		{
			46,
			new OpConstantNull()
		},
		{
			48,
			new OpSpecConstantTrue()
		},
		{
			49,
			new OpSpecConstantFalse()
		},
		{
			50,
			new OpSpecConstant()
		},
		{
			51,
			new OpSpecConstantComposite()
		},
		{
			52,
			new OpSpecConstantOp()
		},
		{
			54,
			new OpFunction()
		},
		{
			55,
			new OpFunctionParameter()
		},
		{
			56,
			new OpFunctionEnd()
		},
		{
			57,
			new OpFunctionCall()
		},
		{
			59,
			new OpVariable()
		},
		{
			60,
			new OpImageTexelPointer()
		},
		{
			61,
			new OpLoad()
		},
		{
			62,
			new OpStore()
		},
		{
			63,
			new OpCopyMemory()
		},
		{
			64,
			new OpCopyMemorySized()
		},
		{
			65,
			new OpAccessChain()
		},
		{
			66,
			new OpInBoundsAccessChain()
		},
		{
			67,
			new OpPtrAccessChain()
		},
		{
			68,
			new OpArrayLength()
		},
		{
			69,
			new OpGenericPtrMemSemantics()
		},
		{
			70,
			new OpInBoundsPtrAccessChain()
		},
		{
			71,
			new OpDecorate()
		},
		{
			72,
			new OpMemberDecorate()
		},
		{
			73,
			new OpDecorationGroup()
		},
		{
			74,
			new OpGroupDecorate()
		},
		{
			75,
			new OpGroupMemberDecorate()
		},
		{
			77,
			new OpVectorExtractDynamic()
		},
		{
			78,
			new OpVectorInsertDynamic()
		},
		{
			79,
			new OpVectorShuffle()
		},
		{
			80,
			new OpCompositeConstruct()
		},
		{
			81,
			new OpCompositeExtract()
		},
		{
			82,
			new OpCompositeInsert()
		},
		{
			83,
			new OpCopyObject()
		},
		{
			84,
			new OpTranspose()
		},
		{
			86,
			new OpSampledImage()
		},
		{
			87,
			new OpImageSampleImplicitLod()
		},
		{
			88,
			new OpImageSampleExplicitLod()
		},
		{
			89,
			new OpImageSampleDrefImplicitLod()
		},
		{
			90,
			new OpImageSampleDrefExplicitLod()
		},
		{
			91,
			new OpImageSampleProjImplicitLod()
		},
		{
			92,
			new OpImageSampleProjExplicitLod()
		},
		{
			93,
			new OpImageSampleProjDrefImplicitLod()
		},
		{
			94,
			new OpImageSampleProjDrefExplicitLod()
		},
		{
			95,
			new OpImageFetch()
		},
		{
			96,
			new OpImageGather()
		},
		{
			97,
			new OpImageDrefGather()
		},
		{
			98,
			new OpImageRead()
		},
		{
			99,
			new OpImageWrite()
		},
		{
			100,
			new OpImage()
		},
		{
			101,
			new OpImageQueryFormat()
		},
		{
			102,
			new OpImageQueryOrder()
		},
		{
			103,
			new OpImageQuerySizeLod()
		},
		{
			104,
			new OpImageQuerySize()
		},
		{
			105,
			new OpImageQueryLod()
		},
		{
			106,
			new OpImageQueryLevels()
		},
		{
			107,
			new OpImageQuerySamples()
		},
		{
			109,
			new OpConvertFToU()
		},
		{
			110,
			new OpConvertFToS()
		},
		{
			111,
			new OpConvertSToF()
		},
		{
			112,
			new OpConvertUToF()
		},
		{
			113,
			new OpUConvert()
		},
		{
			114,
			new OpSConvert()
		},
		{
			115,
			new OpFConvert()
		},
		{
			116,
			new OpQuantizeToF16()
		},
		{
			117,
			new OpConvertPtrToU()
		},
		{
			118,
			new OpSatConvertSToU()
		},
		{
			119,
			new OpSatConvertUToS()
		},
		{
			120,
			new OpConvertUToPtr()
		},
		{
			121,
			new OpPtrCastToGeneric()
		},
		{
			122,
			new OpGenericCastToPtr()
		},
		{
			123,
			new OpGenericCastToPtrExplicit()
		},
		{
			124,
			new OpBitcast()
		},
		{
			126,
			new OpSNegate()
		},
		{
			127,
			new OpFNegate()
		},
		{
			128,
			new OpIAdd()
		},
		{
			129,
			new OpFAdd()
		},
		{
			130,
			new OpISub()
		},
		{
			131,
			new OpFSub()
		},
		{
			132,
			new OpIMul()
		},
		{
			133,
			new OpFMul()
		},
		{
			134,
			new OpUDiv()
		},
		{
			135,
			new OpSDiv()
		},
		{
			136,
			new OpFDiv()
		},
		{
			137,
			new OpUMod()
		},
		{
			138,
			new OpSRem()
		},
		{
			139,
			new OpSMod()
		},
		{
			140,
			new OpFRem()
		},
		{
			141,
			new OpFMod()
		},
		{
			142,
			new OpVectorTimesScalar()
		},
		{
			143,
			new OpMatrixTimesScalar()
		},
		{
			144,
			new OpVectorTimesMatrix()
		},
		{
			145,
			new OpMatrixTimesVector()
		},
		{
			146,
			new OpMatrixTimesMatrix()
		},
		{
			147,
			new OpOuterProduct()
		},
		{
			148,
			new OpDot()
		},
		{
			149,
			new OpIAddCarry()
		},
		{
			150,
			new OpISubBorrow()
		},
		{
			151,
			new OpUMulExtended()
		},
		{
			152,
			new OpSMulExtended()
		},
		{
			154,
			new OpAny()
		},
		{
			155,
			new OpAll()
		},
		{
			156,
			new OpIsNan()
		},
		{
			157,
			new OpIsInf()
		},
		{
			158,
			new OpIsFinite()
		},
		{
			159,
			new OpIsNormal()
		},
		{
			160,
			new OpSignBitSet()
		},
		{
			161,
			new OpLessOrGreater()
		},
		{
			162,
			new OpOrdered()
		},
		{
			163,
			new OpUnordered()
		},
		{
			164,
			new OpLogicalEqual()
		},
		{
			165,
			new OpLogicalNotEqual()
		},
		{
			166,
			new OpLogicalOr()
		},
		{
			167,
			new OpLogicalAnd()
		},
		{
			168,
			new OpLogicalNot()
		},
		{
			169,
			new OpSelect()
		},
		{
			170,
			new OpIEqual()
		},
		{
			171,
			new OpINotEqual()
		},
		{
			172,
			new OpUGreaterThan()
		},
		{
			173,
			new OpSGreaterThan()
		},
		{
			174,
			new OpUGreaterThanEqual()
		},
		{
			175,
			new OpSGreaterThanEqual()
		},
		{
			176,
			new OpULessThan()
		},
		{
			177,
			new OpSLessThan()
		},
		{
			178,
			new OpULessThanEqual()
		},
		{
			179,
			new OpSLessThanEqual()
		},
		{
			180,
			new OpFOrdEqual()
		},
		{
			181,
			new OpFUnordEqual()
		},
		{
			182,
			new OpFOrdNotEqual()
		},
		{
			183,
			new OpFUnordNotEqual()
		},
		{
			184,
			new OpFOrdLessThan()
		},
		{
			185,
			new OpFUnordLessThan()
		},
		{
			186,
			new OpFOrdGreaterThan()
		},
		{
			187,
			new OpFUnordGreaterThan()
		},
		{
			188,
			new OpFOrdLessThanEqual()
		},
		{
			189,
			new OpFUnordLessThanEqual()
		},
		{
			190,
			new OpFOrdGreaterThanEqual()
		},
		{
			191,
			new OpFUnordGreaterThanEqual()
		},
		{
			194,
			new OpShiftRightLogical()
		},
		{
			195,
			new OpShiftRightArithmetic()
		},
		{
			196,
			new OpShiftLeftLogical()
		},
		{
			197,
			new OpBitwiseOr()
		},
		{
			198,
			new OpBitwiseXor()
		},
		{
			199,
			new OpBitwiseAnd()
		},
		{
			200,
			new OpNot()
		},
		{
			201,
			new OpBitFieldInsert()
		},
		{
			202,
			new OpBitFieldSExtract()
		},
		{
			203,
			new OpBitFieldUExtract()
		},
		{
			204,
			new OpBitReverse()
		},
		{
			205,
			new OpBitCount()
		},
		{
			207,
			new OpDPdx()
		},
		{
			208,
			new OpDPdy()
		},
		{
			209,
			new OpFwidth()
		},
		{
			210,
			new OpDPdxFine()
		},
		{
			211,
			new OpDPdyFine()
		},
		{
			212,
			new OpFwidthFine()
		},
		{
			213,
			new OpDPdxCoarse()
		},
		{
			214,
			new OpDPdyCoarse()
		},
		{
			215,
			new OpFwidthCoarse()
		},
		{
			218,
			new OpEmitVertex()
		},
		{
			219,
			new OpEndPrimitive()
		},
		{
			220,
			new OpEmitStreamVertex()
		},
		{
			221,
			new OpEndStreamPrimitive()
		},
		{
			224,
			new OpControlBarrier()
		},
		{
			225,
			new OpMemoryBarrier()
		},
		{
			227,
			new OpAtomicLoad()
		},
		{
			228,
			new OpAtomicStore()
		},
		{
			229,
			new OpAtomicExchange()
		},
		{
			230,
			new OpAtomicCompareExchange()
		},
		{
			231,
			new OpAtomicCompareExchangeWeak()
		},
		{
			232,
			new OpAtomicIIncrement()
		},
		{
			233,
			new OpAtomicIDecrement()
		},
		{
			234,
			new OpAtomicIAdd()
		},
		{
			235,
			new OpAtomicISub()
		},
		{
			236,
			new OpAtomicSMin()
		},
		{
			237,
			new OpAtomicUMin()
		},
		{
			238,
			new OpAtomicSMax()
		},
		{
			239,
			new OpAtomicUMax()
		},
		{
			240,
			new OpAtomicAnd()
		},
		{
			241,
			new OpAtomicOr()
		},
		{
			242,
			new OpAtomicXor()
		},
		{
			245,
			new OpPhi()
		},
		{
			246,
			new OpLoopMerge()
		},
		{
			247,
			new OpSelectionMerge()
		},
		{
			248,
			new OpLabel()
		},
		{
			249,
			new OpBranch()
		},
		{
			250,
			new OpBranchConditional()
		},
		{
			251,
			new OpSwitch()
		},
		{
			252,
			new OpKill()
		},
		{
			253,
			new OpReturn()
		},
		{
			254,
			new OpReturnValue()
		},
		{
			255,
			new OpUnreachable()
		},
		{
			256,
			new OpLifetimeStart()
		},
		{
			257,
			new OpLifetimeStop()
		},
		{
			259,
			new OpGroupAsyncCopy()
		},
		{
			260,
			new OpGroupWaitEvents()
		},
		{
			261,
			new OpGroupAll()
		},
		{
			262,
			new OpGroupAny()
		},
		{
			263,
			new OpGroupBroadcast()
		},
		{
			264,
			new OpGroupIAdd()
		},
		{
			265,
			new OpGroupFAdd()
		},
		{
			266,
			new OpGroupFMin()
		},
		{
			267,
			new OpGroupUMin()
		},
		{
			268,
			new OpGroupSMin()
		},
		{
			269,
			new OpGroupFMax()
		},
		{
			270,
			new OpGroupUMax()
		},
		{
			271,
			new OpGroupSMax()
		},
		{
			274,
			new OpReadPipe()
		},
		{
			275,
			new OpWritePipe()
		},
		{
			276,
			new OpReservedReadPipe()
		},
		{
			277,
			new OpReservedWritePipe()
		},
		{
			278,
			new OpReserveReadPipePackets()
		},
		{
			279,
			new OpReserveWritePipePackets()
		},
		{
			280,
			new OpCommitReadPipe()
		},
		{
			281,
			new OpCommitWritePipe()
		},
		{
			282,
			new OpIsValidReserveId()
		},
		{
			283,
			new OpGetNumPipePackets()
		},
		{
			284,
			new OpGetMaxPipePackets()
		},
		{
			285,
			new OpGroupReserveReadPipePackets()
		},
		{
			286,
			new OpGroupReserveWritePipePackets()
		},
		{
			287,
			new OpGroupCommitReadPipe()
		},
		{
			288,
			new OpGroupCommitWritePipe()
		},
		{
			291,
			new OpEnqueueMarker()
		},
		{
			292,
			new OpEnqueueKernel()
		},
		{
			293,
			new OpGetKernelNDrangeSubGroupCount()
		},
		{
			294,
			new OpGetKernelNDrangeMaxSubGroupSize()
		},
		{
			295,
			new OpGetKernelWorkGroupSize()
		},
		{
			296,
			new OpGetKernelPreferredWorkGroupSizeMultiple()
		},
		{
			297,
			new OpRetainEvent()
		},
		{
			298,
			new OpReleaseEvent()
		},
		{
			299,
			new OpCreateUserEvent()
		},
		{
			300,
			new OpIsValidEvent()
		},
		{
			301,
			new OpSetUserEventStatus()
		},
		{
			302,
			new OpCaptureEventProfilingInfo()
		},
		{
			303,
			new OpGetDefaultQueue()
		},
		{
			304,
			new OpBuildNDRange()
		},
		{
			305,
			new OpImageSparseSampleImplicitLod()
		},
		{
			306,
			new OpImageSparseSampleExplicitLod()
		},
		{
			307,
			new OpImageSparseSampleDrefImplicitLod()
		},
		{
			308,
			new OpImageSparseSampleDrefExplicitLod()
		},
		{
			309,
			new OpImageSparseSampleProjImplicitLod()
		},
		{
			310,
			new OpImageSparseSampleProjExplicitLod()
		},
		{
			311,
			new OpImageSparseSampleProjDrefImplicitLod()
		},
		{
			312,
			new OpImageSparseSampleProjDrefExplicitLod()
		},
		{
			313,
			new OpImageSparseFetch()
		},
		{
			314,
			new OpImageSparseGather()
		},
		{
			315,
			new OpImageSparseDrefGather()
		},
		{
			316,
			new OpImageSparseTexelsResident()
		},
		{
			317,
			new OpNoLine()
		},
		{
			318,
			new OpAtomicFlagTestAndSet()
		},
		{
			319,
			new OpAtomicFlagClear()
		},
		{
			320,
			new OpImageSparseRead()
		},
		{
			321,
			new OpSizeOf()
		},
		{
			322,
			new OpTypePipeStorage()
		},
		{
			323,
			new OpConstantPipeStorage()
		},
		{
			324,
			new OpCreatePipeFromPipeStorage()
		},
		{
			325,
			new OpGetKernelLocalSizeForSubgroupCount()
		},
		{
			326,
			new OpGetKernelMaxNumSubgroups()
		},
		{
			327,
			new OpTypeNamedBarrier()
		},
		{
			328,
			new OpNamedBarrierInitialize()
		},
		{
			329,
			new OpMemoryNamedBarrier()
		},
		{
			330,
			new OpModuleProcessed()
		},
		{
			331,
			new OpExecutionModeId()
		},
		{
			332,
			new OpDecorateId()
		},
		{
			4421,
			new OpSubgroupBallotKHR()
		},
		{
			4422,
			new OpSubgroupFirstInvocationKHR()
		},
		{
			4428,
			new OpSubgroupAllKHR()
		},
		{
			4429,
			new OpSubgroupAnyKHR()
		},
		{
			4430,
			new OpSubgroupAllEqualKHR()
		},
		{
			4432,
			new OpSubgroupReadInvocationKHR()
		},
		{
			5000,
			new OpGroupIAddNonUniformAMD()
		},
		{
			5001,
			new OpGroupFAddNonUniformAMD()
		},
		{
			5002,
			new OpGroupFMinNonUniformAMD()
		},
		{
			5003,
			new OpGroupUMinNonUniformAMD()
		},
		{
			5004,
			new OpGroupSMinNonUniformAMD()
		},
		{
			5005,
			new OpGroupFMaxNonUniformAMD()
		},
		{
			5006,
			new OpGroupUMaxNonUniformAMD()
		},
		{
			5007,
			new OpGroupSMaxNonUniformAMD()
		},
		{
			5011,
			new OpFragmentMaskFetchAMD()
		},
		{
			5012,
			new OpFragmentFetchAMD()
		},
		{
			5571,
			new OpSubgroupShuffleINTEL()
		},
		{
			5572,
			new OpSubgroupShuffleDownINTEL()
		},
		{
			5573,
			new OpSubgroupShuffleUpINTEL()
		},
		{
			5574,
			new OpSubgroupShuffleXorINTEL()
		},
		{
			5575,
			new OpSubgroupBlockReadINTEL()
		},
		{
			5576,
			new OpSubgroupBlockWriteINTEL()
		},
		{
			5577,
			new OpSubgroupImageBlockReadINTEL()
		},
		{
			5578,
			new OpSubgroupImageBlockWriteINTEL()
		}
	};

	public static IReadOnlyDictionary<int, Instruction> OpcodeToInstruction => instructions_;
}
