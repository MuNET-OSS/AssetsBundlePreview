using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace SpirV;

public class Module
{
	[StructLayout(LayoutKind.Explicit)]
	private struct FloatUIntUnion
	{
		[FieldOffset(0)]
		public uint Int;

		[FieldOffset(0)]
		public float Float;
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct DoubleULongUnion
	{
		[FieldOffset(0)]
		public ulong Long;

		[FieldOffset(0)]
		public double Double;
	}

	private static HashSet<string> debugInstructions_ = new HashSet<string> { "OpSourceContinued", "OpSource", "OpSourceExtension", "OpName", "OpMemberName", "OpString", "OpLine", "OpNoLine", "OpModuleProcessed" };

	private readonly Dictionary<uint, ParsedInstruction> objects_ = new Dictionary<uint, ParsedInstruction>();

	public ModuleHeader Header { get; }

	public IReadOnlyList<ParsedInstruction> Instructions { get; }

	public Module(ModuleHeader header, IReadOnlyList<ParsedInstruction> instructions)
	{
		Header = header;
		Instructions = instructions;
		Read(Instructions, objects_);
	}

	public static bool IsDebugInstruction(ParsedInstruction instruction)
	{
		return debugInstructions_.Contains(instruction.Instruction.Name);
	}

	private static void Read(IReadOnlyList<ParsedInstruction> instructions, Dictionary<uint, ParsedInstruction> objects)
	{
		List<ParsedInstruction> debugInstructions = new List<ParsedInstruction>();
		List<ParsedInstruction> entryPoints = new List<ParsedInstruction>();
		foreach (ParsedInstruction instruction2 in instructions)
		{
			if (IsDebugInstruction(instruction2))
			{
				debugInstructions.Add(instruction2);
				continue;
			}
			if (instruction2.Instruction is OpEntryPoint)
			{
				entryPoints.Add(instruction2);
				continue;
			}
			if (instruction2.Instruction.Name.StartsWith("OpType", StringComparison.Ordinal))
			{
				ProcessTypeInstruction(instruction2, objects);
			}
			instruction2.ResolveResultType(objects);
			if (instruction2.HasResult)
			{
				objects[instruction2.ResultId] = instruction2;
			}
			Instruction instruction3 = instruction2.Instruction;
			if (instruction3 is OpSpecConstant || instruction3 is OpConstant)
			{
				_ = instruction2.ResultType;
				object constant = ConvertConstant(instruction2.ResultType as ScalarType, instruction2.Words, 3);
				instruction2.Operands[2].Value = constant;
				instruction2.Value = constant;
			}
		}
		foreach (ParsedInstruction instruction in debugInstructions)
		{
			Instruction instruction3 = instruction.Instruction;
			if (!(instruction3 is OpMemberName))
			{
				if (instruction3 is OpName)
				{
					objects[instruction.Words[1]].Name = (string)instruction.Operands[1].Value;
				}
			}
			else
			{
				((StructType)objects[instruction.Words[1]].ResultType).SetMemberName((uint)instruction.Operands[1].Value, (string)instruction.Operands[2].Value);
			}
		}
		foreach (ParsedInstruction instruction4 in instructions)
		{
			instruction4.ResolveReferences(objects);
		}
	}

	public static Module ReadFrom(Stream stream)
	{
		Reader reader = new Reader(new BinaryReader(stream));
		uint num = reader.ReadDWord();
		int majorVersion = (int)(num >> 16);
		int minorVersion = (int)((num >> 8) & 0xFF);
		Version version = new Version(majorVersion, minorVersion);
		uint generatorMagicNumber = reader.ReadDWord();
		int generatorToolId = (int)(generatorMagicNumber >> 16);
		string generatorVendor = "unknown";
		string generatorName = null;
		if (Meta.Tools.ContainsKey(generatorToolId))
		{
			Meta.ToolInfo toolInfo = Meta.Tools[generatorToolId];
			generatorVendor = toolInfo.Vendor;
			if (toolInfo.Name != null)
			{
				generatorName = toolInfo.Name;
			}
		}
		ModuleHeader header = default(ModuleHeader);
		header.Version = version;
		header.GeneratorName = generatorName;
		header.GeneratorVendor = generatorVendor;
		header.GeneratorVersion = (int)(generatorMagicNumber & 0xFFFF);
		header.Bound = reader.ReadDWord();
		header.Reserved = reader.ReadDWord();
		List<ParsedInstruction> instructions = new List<ParsedInstruction>();
		while (!reader.EndOfStream)
		{
			uint instructionStart = reader.ReadDWord();
			ushort wordCount = (ushort)(instructionStart >> 16);
			int opCode = (int)(instructionStart & 0xFFFF);
			uint[] words = new uint[wordCount];
			words[0] = instructionStart;
			for (ushort i = 1; i < wordCount; i++)
			{
				words[i] = reader.ReadDWord();
			}
			ParsedInstruction instruction = new ParsedInstruction(opCode, words);
			instructions.Add(instruction);
		}
		return new Module(header, instructions);
	}

	private static void ProcessTypeInstruction(ParsedInstruction i, IReadOnlyDictionary<uint, ParsedInstruction> objects)
	{
		Instruction instruction = i.Instruction;
		if (!(instruction is OpTypeInt))
		{
			if (!(instruction is OpTypeFloat))
			{
				if (!(instruction is OpTypeVector))
				{
					if (!(instruction is OpTypeMatrix))
					{
						if (!(instruction is OpTypeArray))
						{
							if (!(instruction is OpTypeRuntimeArray))
							{
								if (!(instruction is OpTypeBool))
								{
									if (!(instruction is OpTypeOpaque))
									{
										if (!(instruction is OpTypeVoid))
										{
											if (!(instruction is OpTypeImage))
											{
												if (!(instruction is OpTypeSampler))
												{
													if (!(instruction is OpTypeSampledImage))
													{
														if (!(instruction is OpTypeFunction))
														{
															if (!(instruction is OpTypeForwardPointer))
															{
																if (!(instruction is OpTypePointer))
																{
																	if (instruction is OpTypeStruct)
																	{
																		List<Type> memberTypes = new List<Type>();
																		for (int j = 2; j < i.Words.Count; j++)
																		{
																			memberTypes.Add(objects[i.Words[j]].ResultType);
																		}
																		i.ResultType = new StructType(memberTypes);
																	}
																}
																else if (objects.ContainsKey(i.Words[1]))
																{
																	((PointerType)i.ResultType).ResolveForwardReference(objects[i.Words[3]].ResultType);
																}
																else
																{
																	i.ResultType = new PointerType((StorageClass)i.Words[2], objects[i.Words[3]].ResultType);
																}
															}
															else
															{
																i.ResultType = new PointerType((StorageClass)i.Words[2]);
															}
														}
														else
														{
															List<Type> parameterTypes = new List<Type>();
															for (int k = 3; k < i.Words.Count; k++)
															{
																parameterTypes.Add(objects[i.Words[k]].ResultType);
															}
															i.ResultType = new FunctionType(objects[i.Words[2]].ResultType, parameterTypes);
														}
													}
													else
													{
														i.ResultType = new SampledImageType((ImageType)objects[i.Words[2]].ResultType);
													}
												}
												else
												{
													i.ResultType = new SamplerType();
												}
											}
											else
											{
												Type sampledType = objects[i.Operands[1].GetId()].ResultType;
												Dim dim = i.Operands[2].GetSingleEnumValue<Dim>();
												uint depth = (uint)i.Operands[3].Value;
												bool isArray = (uint)i.Operands[4].Value != 0;
												bool isMultiSampled = (uint)i.Operands[5].Value != 0;
												uint sampled = (uint)i.Operands[6].Value;
												ImageFormat imageFormat = i.Operands[7].GetSingleEnumValue<ImageFormat>();
												i.ResultType = new ImageType(sampledType, dim, (int)depth, isArray, isMultiSampled, (int)sampled, imageFormat, (i.Operands.Count > 8) ? i.Operands[8].GetSingleEnumValue<AccessQualifier>() : AccessQualifier.ReadOnly);
											}
										}
										else
										{
											i.ResultType = new VoidType();
										}
									}
									else
									{
										i.ResultType = new OpaqueType();
									}
								}
								else
								{
									i.ResultType = new BoolType();
								}
							}
							else
							{
								i.ResultType = new RuntimeArrayType(objects[i.Words[2]].ResultType);
							}
							return;
						}
						object constant = objects[i.Words[3]].Value;
						int size = 0;
						if (!(constant is ushort u16))
						{
							if (!(constant is uint u17))
							{
								if (!(constant is ulong u18))
								{
									if (!(constant is short i2))
									{
										if (!(constant is int i3))
										{
											if (constant is long i4)
											{
												size = (int)i4;
											}
										}
										else
										{
											size = i3;
										}
									}
									else
									{
										size = i2;
									}
								}
								else
								{
									size = (int)u18;
								}
							}
							else
							{
								size = (int)u17;
							}
						}
						else
						{
							size = u16;
						}
						i.ResultType = new ArrayType(objects[i.Words[2]].ResultType, size);
					}
					else
					{
						i.ResultType = new MatrixType((VectorType)objects[i.Words[2]].ResultType, (int)i.Words[3]);
					}
				}
				else
				{
					i.ResultType = new VectorType((ScalarType)objects[i.Words[2]].ResultType, (int)i.Words[3]);
				}
			}
			else
			{
				i.ResultType = new FloatingPointType((int)i.Words[2]);
			}
		}
		else
		{
			i.ResultType = new IntegerType((int)i.Words[2], i.Words[3] == 1);
		}
	}

	private static object ConvertConstant(ScalarType type, IReadOnlyList<uint> words, int index)
	{
		if (!(type is IntegerType i))
		{
			if (type is FloatingPointType f)
			{
				if (f.Width == 32)
				{
					FloatUIntUnion floatUIntUnion = default(FloatUIntUnion);
					floatUIntUnion.Int = words[0];
					return floatUIntUnion.Float;
				}
				if (f.Width == 64)
				{
					DoubleULongUnion doubleULongUnion = default(DoubleULongUnion);
					doubleULongUnion.Long = words[index] | ((ulong)words[index + 1] << 32);
					return doubleULongUnion.Double;
				}
				throw new Exception("Cannot construct floating point literal.");
			}
			return null;
		}
		if (i.Signed)
		{
			if (i.Width == 16)
			{
				return (short)words[index];
			}
			if (i.Width == 32)
			{
				return (int)words[index];
			}
			if (i.Width == 64)
			{
				return (long)(words[index] | ((ulong)words[index + 1] << 32));
			}
		}
		else
		{
			if (i.Width == 16)
			{
				return (ushort)words[index];
			}
			if (i.Width == 32)
			{
				return words[index];
			}
			if (i.Width == 64)
			{
				return words[index] | ((ulong)words[index + 1] << 32);
			}
		}
		throw new Exception("Cannot construct integer literal.");
	}
}
