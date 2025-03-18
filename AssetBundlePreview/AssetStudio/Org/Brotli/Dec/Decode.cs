using System;

namespace Org.Brotli.Dec;

internal sealed class Decode
{
	private const int DefaultCodeLength = 8;

	private const int CodeLengthRepeatCode = 16;

	private const int NumLiteralCodes = 256;

	private const int NumInsertAndCopyCodes = 704;

	private const int NumBlockLengthCodes = 26;

	private const int LiteralContextBits = 6;

	private const int DistanceContextBits = 2;

	private const int HuffmanTableBits = 8;

	private const int HuffmanTableMask = 255;

	private const int CodeLengthCodes = 18;

	private static readonly int[] CodeLengthCodeOrder = new int[18]
	{
		1, 2, 3, 4, 0, 5, 17, 6, 16, 7,
		8, 9, 10, 11, 12, 13, 14, 15
	};

	private const int NumDistanceShortCodes = 16;

	private static readonly int[] DistanceShortCodeIndexOffset = new int[16]
	{
		3, 2, 1, 0, 3, 3, 3, 3, 3, 3,
		2, 2, 2, 2, 2, 2
	};

	private static readonly int[] DistanceShortCodeValueOffset = new int[16]
	{
		0, 0, 0, 0, -1, 1, -2, 2, -3, 3,
		-1, 1, -2, 2, -3, 3
	};

	private static readonly int[] FixedTable = new int[16]
	{
		131072, 131076, 131075, 196610, 131072, 131076, 131075, 262145, 131072, 131076,
		131075, 196610, 131072, 131076, 131075, 262149
	};

	private static int DecodeVarLenUnsignedByte(BitReader br)
	{
		if (BitReader.ReadBits(br, 1) != 0)
		{
			int i = BitReader.ReadBits(br, 3);
			if (i == 0)
			{
				return 1;
			}
			return BitReader.ReadBits(br, i) + (1 << i);
		}
		return 0;
	}

	private static void DecodeMetaBlockLength(BitReader br, State state)
	{
		state.inputEnd = BitReader.ReadBits(br, 1) == 1;
		state.metaBlockLength = 0;
		state.isUncompressed = false;
		state.isMetadata = false;
		if (state.inputEnd && BitReader.ReadBits(br, 1) != 0)
		{
			return;
		}
		int sizeNibbles = BitReader.ReadBits(br, 2) + 4;
		if (sizeNibbles == 7)
		{
			state.isMetadata = true;
			if (BitReader.ReadBits(br, 1) != 0)
			{
				throw new BrotliRuntimeException("Corrupted reserved bit");
			}
			int sizeBytes = BitReader.ReadBits(br, 2);
			if (sizeBytes == 0)
			{
				return;
			}
			for (int i = 0; i < sizeBytes; i++)
			{
				int bits = BitReader.ReadBits(br, 8);
				if (bits == 0 && i + 1 == sizeBytes && sizeBytes > 1)
				{
					throw new BrotliRuntimeException("Exuberant nibble");
				}
				state.metaBlockLength |= bits << i * 8;
			}
		}
		else
		{
			for (int j = 0; j < sizeNibbles; j++)
			{
				int bits2 = BitReader.ReadBits(br, 4);
				if (bits2 == 0 && j + 1 == sizeNibbles && sizeNibbles > 4)
				{
					throw new BrotliRuntimeException("Exuberant nibble");
				}
				state.metaBlockLength |= bits2 << j * 4;
			}
		}
		state.metaBlockLength++;
		if (!state.inputEnd)
		{
			state.isUncompressed = BitReader.ReadBits(br, 1) == 1;
		}
	}

	private static int ReadSymbol(int[] table, int offset, BitReader br)
	{
		int val = (int)(br.accumulator >>> br.bitOffset);
		offset += val & 0xFF;
		int bits = table[offset] >> 16;
		int sym = table[offset] & 0xFFFF;
		if (bits <= 8)
		{
			br.bitOffset += bits;
			return sym;
		}
		offset += sym;
		int mask = (1 << bits) - 1;
		offset += (val & mask) >>> 8;
		br.bitOffset += (table[offset] >> 16) + 8;
		return table[offset] & 0xFFFF;
	}

	private static int ReadBlockLength(int[] table, int offset, BitReader br)
	{
		BitReader.FillBitWindow(br);
		int code = ReadSymbol(table, offset, br);
		int i = Prefix.BlockLengthNBits[code];
		return Prefix.BlockLengthOffset[code] + BitReader.ReadBits(br, i);
	}

	private static int TranslateShortCodes(int code, int[] ringBuffer, int index)
	{
		if (code < 16)
		{
			index += DistanceShortCodeIndexOffset[code];
			index &= 3;
			return ringBuffer[index] + DistanceShortCodeValueOffset[code];
		}
		return code - 16 + 1;
	}

	private static void MoveToFront(int[] v, int index)
	{
		int value = v[index];
		while (index > 0)
		{
			v[index] = v[index - 1];
			index--;
		}
		v[0] = value;
	}

	private static void InverseMoveToFrontTransform(byte[] v, int vLen)
	{
		int[] mtf = new int[256];
		for (int j = 0; j < 256; j++)
		{
			mtf[j] = j;
		}
		for (int i = 0; i < vLen; i++)
		{
			int index = v[i] & 0xFF;
			v[i] = (byte)mtf[index];
			if (index != 0)
			{
				MoveToFront(mtf, index);
			}
		}
	}

	private static void ReadHuffmanCodeLengths(int[] codeLengthCodeLengths, int numSymbols, int[] codeLengths, BitReader br)
	{
		int symbol = 0;
		int prevCodeLen = 8;
		int repeat = 0;
		int repeatCodeLen = 0;
		int space = 32768;
		int[] table = new int[32];
		Huffman.BuildHuffmanTable(table, 0, 5, codeLengthCodeLengths, 18);
		while (symbol < numSymbols && space > 0)
		{
			BitReader.ReadMoreInput(br);
			BitReader.FillBitWindow(br);
			int p = (int)(br.accumulator >>> br.bitOffset) & 0x1F;
			br.bitOffset += table[p] >> 16;
			int codeLen = table[p] & 0xFFFF;
			if (codeLen < 16)
			{
				repeat = 0;
				codeLengths[symbol++] = codeLen;
				if (codeLen != 0)
				{
					prevCodeLen = codeLen;
					space -= 32768 >> codeLen;
				}
				continue;
			}
			int extraBits = codeLen - 14;
			int newLen = 0;
			if (codeLen == 16)
			{
				newLen = prevCodeLen;
			}
			if (repeatCodeLen != newLen)
			{
				repeat = 0;
				repeatCodeLen = newLen;
			}
			int oldRepeat = repeat;
			if (repeat > 0)
			{
				repeat -= 2;
				repeat <<= extraBits;
			}
			repeat += BitReader.ReadBits(br, extraBits) + 3;
			int repeatDelta = repeat - oldRepeat;
			if (symbol + repeatDelta > numSymbols)
			{
				throw new BrotliRuntimeException("symbol + repeatDelta > numSymbols");
			}
			for (int i = 0; i < repeatDelta; i++)
			{
				codeLengths[symbol++] = repeatCodeLen;
			}
			if (repeatCodeLen != 0)
			{
				space -= repeatDelta << 15 - repeatCodeLen;
			}
		}
		if (space != 0)
		{
			throw new BrotliRuntimeException("Unused space");
		}
		Utils.FillWithZeroes(codeLengths, symbol, numSymbols - symbol);
	}

	internal static void ReadHuffmanCode(int alphabetSize, int[] table, int offset, BitReader br)
	{
		bool ok = true;
		BitReader.ReadMoreInput(br);
		int[] codeLengths = new int[alphabetSize];
		int simpleCodeOrSkip = BitReader.ReadBits(br, 2);
		if (simpleCodeOrSkip == 1)
		{
			int maxBitsCounter = alphabetSize - 1;
			int maxBits = 0;
			int[] symbols = new int[4];
			int numSymbols = BitReader.ReadBits(br, 2) + 1;
			while (maxBitsCounter != 0)
			{
				maxBitsCounter >>= 1;
				maxBits++;
			}
			for (int i = 0; i < numSymbols; i++)
			{
				symbols[i] = BitReader.ReadBits(br, maxBits) % alphabetSize;
				codeLengths[symbols[i]] = 2;
			}
			codeLengths[symbols[0]] = 1;
			switch (numSymbols)
			{
			case 2:
				ok = symbols[0] != symbols[1];
				codeLengths[symbols[1]] = 1;
				break;
			case 3:
				ok = symbols[0] != symbols[1] && symbols[0] != symbols[2] && symbols[1] != symbols[2];
				break;
			default:
				ok = symbols[0] != symbols[1] && symbols[0] != symbols[2] && symbols[0] != symbols[3] && symbols[1] != symbols[2] && symbols[1] != symbols[3] && symbols[2] != symbols[3];
				if (BitReader.ReadBits(br, 1) == 1)
				{
					codeLengths[symbols[2]] = 3;
					codeLengths[symbols[3]] = 3;
				}
				else
				{
					codeLengths[symbols[0]] = 2;
				}
				break;
			case 1:
				break;
			}
		}
		else
		{
			int[] codeLengthCodeLengths = new int[18];
			int space = 32;
			int numCodes = 0;
			for (int j = simpleCodeOrSkip; j < 18; j++)
			{
				if (space <= 0)
				{
					break;
				}
				int codeLenIdx = CodeLengthCodeOrder[j];
				BitReader.FillBitWindow(br);
				int p = (int)(br.accumulator >>> br.bitOffset) & 0xF;
				br.bitOffset += FixedTable[p] >> 16;
				int v = (codeLengthCodeLengths[codeLenIdx] = FixedTable[p] & 0xFFFF);
				if (v != 0)
				{
					space -= 32 >> v;
					numCodes++;
				}
			}
			ok = numCodes == 1 || space == 0;
			ReadHuffmanCodeLengths(codeLengthCodeLengths, alphabetSize, codeLengths, br);
		}
		if (!ok)
		{
			throw new BrotliRuntimeException("Can't readHuffmanCode");
		}
		Huffman.BuildHuffmanTable(table, offset, 8, codeLengths, alphabetSize);
	}

	private static int DecodeContextMap(int contextMapSize, byte[] contextMap, BitReader br)
	{
		BitReader.ReadMoreInput(br);
		int numTrees = DecodeVarLenUnsignedByte(br) + 1;
		if (numTrees == 1)
		{
			Utils.FillWithZeroes(contextMap, 0, contextMapSize);
			return numTrees;
		}
		bool num = BitReader.ReadBits(br, 1) == 1;
		int maxRunLengthPrefix = 0;
		if (num)
		{
			maxRunLengthPrefix = BitReader.ReadBits(br, 4) + 1;
		}
		int[] table = new int[1080];
		ReadHuffmanCode(numTrees + maxRunLengthPrefix, table, 0, br);
		int i = 0;
		while (i < contextMapSize)
		{
			BitReader.ReadMoreInput(br);
			BitReader.FillBitWindow(br);
			int code = ReadSymbol(table, 0, br);
			if (code == 0)
			{
				contextMap[i] = 0;
				i++;
			}
			else if (code <= maxRunLengthPrefix)
			{
				for (int reps = (1 << code) + BitReader.ReadBits(br, code); reps != 0; reps--)
				{
					if (i >= contextMapSize)
					{
						throw new BrotliRuntimeException("Corrupted context map");
					}
					contextMap[i] = 0;
					i++;
				}
			}
			else
			{
				contextMap[i] = (byte)(code - maxRunLengthPrefix);
				i++;
			}
		}
		if (BitReader.ReadBits(br, 1) == 1)
		{
			InverseMoveToFrontTransform(contextMap, contextMapSize);
		}
		return numTrees;
	}

	private static void DecodeBlockTypeAndLength(State state, int treeType)
	{
		BitReader br = state.br;
		int[] ringBuffers = state.blockTypeRb;
		int offset = treeType * 2;
		BitReader.FillBitWindow(br);
		int blockType = ReadSymbol(state.blockTypeTrees, treeType * 1080, br);
		state.blockLength[treeType] = ReadBlockLength(state.blockLenTrees, treeType * 1080, br);
		blockType = blockType switch
		{
			1 => ringBuffers[offset + 1] + 1, 
			0 => ringBuffers[offset], 
			_ => blockType - 2, 
		};
		if (blockType >= state.numBlockTypes[treeType])
		{
			blockType -= state.numBlockTypes[treeType];
		}
		ringBuffers[offset] = ringBuffers[offset + 1];
		ringBuffers[offset + 1] = blockType;
	}

	private static void DecodeLiteralBlockSwitch(State state)
	{
		DecodeBlockTypeAndLength(state, 0);
		int literalBlockType = state.blockTypeRb[1];
		state.contextMapSlice = literalBlockType << 6;
		state.literalTreeIndex = state.contextMap[state.contextMapSlice] & 0xFF;
		state.literalTree = state.hGroup0.trees[state.literalTreeIndex];
		int contextMode = state.contextModes[literalBlockType];
		state.contextLookupOffset1 = Context.LookupOffsets[contextMode];
		state.contextLookupOffset2 = Context.LookupOffsets[contextMode + 1];
	}

	private static void DecodeCommandBlockSwitch(State state)
	{
		DecodeBlockTypeAndLength(state, 1);
		state.treeCommandOffset = state.hGroup1.trees[state.blockTypeRb[3]];
	}

	private static void DecodeDistanceBlockSwitch(State state)
	{
		DecodeBlockTypeAndLength(state, 2);
		state.distContextMapSlice = state.blockTypeRb[5] << 2;
	}

	private static void MaybeReallocateRingBuffer(State state)
	{
		int newSize = state.maxRingBufferSize;
		if (newSize > state.expectedTotalSize)
		{
			int minimalNewSize = (int)state.expectedTotalSize + state.customDictionary.Length;
			while (newSize >> 1 > minimalNewSize)
			{
				newSize >>= 1;
			}
			if (!state.inputEnd && newSize < 16384 && state.maxRingBufferSize >= 16384)
			{
				newSize = 16384;
			}
		}
		if (newSize <= state.ringBufferSize)
		{
			return;
		}
		byte[] newBuffer = new byte[newSize + 37];
		if (state.ringBuffer != null)
		{
			Array.Copy(state.ringBuffer, 0, newBuffer, 0, state.ringBufferSize);
		}
		else if (state.customDictionary.Length != 0)
		{
			int length = state.customDictionary.Length;
			int offset = 0;
			if (length > state.maxBackwardDistance)
			{
				offset = length - state.maxBackwardDistance;
				length = state.maxBackwardDistance;
			}
			Array.Copy(state.customDictionary, offset, newBuffer, 0, length);
			state.pos = length;
			state.bytesToIgnore = length;
		}
		state.ringBuffer = newBuffer;
		state.ringBufferSize = newSize;
	}

	private static void ReadMetablockInfo(State state)
	{
		BitReader br = state.br;
		if (state.inputEnd)
		{
			state.nextRunningState = 10;
			state.bytesToWrite = state.pos;
			state.bytesWritten = 0;
			state.runningState = 12;
			return;
		}
		state.hGroup0.codes = null;
		state.hGroup0.trees = null;
		state.hGroup1.codes = null;
		state.hGroup1.trees = null;
		state.hGroup2.codes = null;
		state.hGroup2.trees = null;
		BitReader.ReadMoreInput(br);
		DecodeMetaBlockLength(br, state);
		if (state.metaBlockLength == 0 && !state.isMetadata)
		{
			return;
		}
		if (state.isUncompressed || state.isMetadata)
		{
			BitReader.JumpToByteBoundary(br);
			state.runningState = (state.isMetadata ? 4 : 5);
		}
		else
		{
			state.runningState = 2;
		}
		if (!state.isMetadata)
		{
			state.expectedTotalSize += state.metaBlockLength;
			if (state.ringBufferSize < state.maxRingBufferSize)
			{
				MaybeReallocateRingBuffer(state);
			}
		}
	}

	private static void ReadMetablockHuffmanCodesAndContextMaps(State state)
	{
		BitReader br = state.br;
		for (int i = 0; i < 3; i++)
		{
			state.numBlockTypes[i] = DecodeVarLenUnsignedByte(br) + 1;
			state.blockLength[i] = 268435456;
			if (state.numBlockTypes[i] > 1)
			{
				ReadHuffmanCode(state.numBlockTypes[i] + 2, state.blockTypeTrees, i * 1080, br);
				ReadHuffmanCode(26, state.blockLenTrees, i * 1080, br);
				state.blockLength[i] = ReadBlockLength(state.blockLenTrees, i * 1080, br);
			}
		}
		BitReader.ReadMoreInput(br);
		state.distancePostfixBits = BitReader.ReadBits(br, 2);
		state.numDirectDistanceCodes = 16 + (BitReader.ReadBits(br, 4) << state.distancePostfixBits);
		state.distancePostfixMask = (1 << state.distancePostfixBits) - 1;
		int numDistanceCodes = state.numDirectDistanceCodes + (48 << state.distancePostfixBits);
		state.contextModes = new byte[state.numBlockTypes[0]];
		int j = 0;
		while (j < state.numBlockTypes[0])
		{
			for (int limit = Math.Min(j + 96, state.numBlockTypes[0]); j < limit; j++)
			{
				state.contextModes[j] = (byte)(BitReader.ReadBits(br, 2) << 1);
			}
			BitReader.ReadMoreInput(br);
		}
		state.contextMap = new byte[state.numBlockTypes[0] << 6];
		int numLiteralTrees = DecodeContextMap(state.numBlockTypes[0] << 6, state.contextMap, br);
		state.trivialLiteralContext = true;
		for (int k = 0; k < state.numBlockTypes[0] << 6; k++)
		{
			if (state.contextMap[k] != k >> 6)
			{
				state.trivialLiteralContext = false;
				break;
			}
		}
		state.distContextMap = new byte[state.numBlockTypes[2] << 2];
		int numDistTrees = DecodeContextMap(state.numBlockTypes[2] << 2, state.distContextMap, br);
		HuffmanTreeGroup.Init(state.hGroup0, 256, numLiteralTrees);
		HuffmanTreeGroup.Init(state.hGroup1, 704, state.numBlockTypes[1]);
		HuffmanTreeGroup.Init(state.hGroup2, numDistanceCodes, numDistTrees);
		HuffmanTreeGroup.Decode(state.hGroup0, br);
		HuffmanTreeGroup.Decode(state.hGroup1, br);
		HuffmanTreeGroup.Decode(state.hGroup2, br);
		state.contextMapSlice = 0;
		state.distContextMapSlice = 0;
		state.contextLookupOffset1 = Context.LookupOffsets[state.contextModes[0]];
		state.contextLookupOffset2 = Context.LookupOffsets[state.contextModes[0] + 1];
		state.literalTreeIndex = 0;
		state.literalTree = state.hGroup0.trees[0];
		state.treeCommandOffset = state.hGroup1.trees[0];
		state.blockTypeRb[0] = (state.blockTypeRb[2] = (state.blockTypeRb[4] = 1));
		state.blockTypeRb[1] = (state.blockTypeRb[3] = (state.blockTypeRb[5] = 0));
	}

	private static void CopyUncompressedData(State state)
	{
		BitReader br = state.br;
		byte[] ringBuffer = state.ringBuffer;
		if (state.metaBlockLength <= 0)
		{
			BitReader.Reload(br);
			state.runningState = 1;
			return;
		}
		int chunkLength = Math.Min(state.ringBufferSize - state.pos, state.metaBlockLength);
		BitReader.CopyBytes(br, ringBuffer, state.pos, chunkLength);
		state.metaBlockLength -= chunkLength;
		state.pos += chunkLength;
		if (state.pos == state.ringBufferSize)
		{
			state.nextRunningState = 5;
			state.bytesToWrite = state.ringBufferSize;
			state.bytesWritten = 0;
			state.runningState = 12;
		}
		else
		{
			BitReader.Reload(br);
			state.runningState = 1;
		}
	}

	private static bool WriteRingBuffer(State state)
	{
		if (state.bytesToIgnore != 0)
		{
			state.bytesWritten += state.bytesToIgnore;
			state.bytesToIgnore = 0;
		}
		int toWrite = Math.Min(state.outputLength - state.outputUsed, state.bytesToWrite - state.bytesWritten);
		if (toWrite != 0)
		{
			Array.Copy(state.ringBuffer, state.bytesWritten, state.output, state.outputOffset + state.outputUsed, toWrite);
			state.outputUsed += toWrite;
			state.bytesWritten += toWrite;
		}
		return state.outputUsed < state.outputLength;
	}

	internal static void SetCustomDictionary(State state, byte[] data)
	{
		state.customDictionary = ((data == null) ? new byte[0] : data);
	}

	internal static void Decompress(State state)
	{
		if (state.runningState == 0)
		{
			throw new InvalidOperationException("Can't decompress until initialized");
		}
		if (state.runningState == 11)
		{
			throw new InvalidOperationException("Can't decompress after close");
		}
		BitReader br = state.br;
		int ringBufferMask = state.ringBufferSize - 1;
		byte[] ringBuffer = state.ringBuffer;
		while (state.runningState != 10)
		{
			switch (state.runningState)
			{
			case 1:
				if (state.metaBlockLength < 0)
				{
					throw new BrotliRuntimeException("Invalid metablock length");
				}
				ReadMetablockInfo(state);
				ringBufferMask = state.ringBufferSize - 1;
				ringBuffer = state.ringBuffer;
				break;
			case 2:
				ReadMetablockHuffmanCodesAndContextMaps(state);
				state.runningState = 3;
				goto case 3;
			case 3:
			{
				if (state.metaBlockLength <= 0)
				{
					state.runningState = 1;
					break;
				}
				BitReader.ReadMoreInput(br);
				if (state.blockLength[1] == 0)
				{
					DecodeCommandBlockSwitch(state);
				}
				state.blockLength[1]--;
				BitReader.FillBitWindow(br);
				int cmdCode = ReadSymbol(state.hGroup1.codes, state.treeCommandOffset, br);
				int rangeIdx = cmdCode >>> 6;
				state.distanceCode = 0;
				if (rangeIdx >= 2)
				{
					rangeIdx -= 2;
					state.distanceCode = -1;
				}
				int insertCode = Prefix.InsertRangeLut[rangeIdx] + ((cmdCode >>> 3) & 7);
				int copyCode = Prefix.CopyRangeLut[rangeIdx] + (cmdCode & 7);
				state.insertLength = Prefix.InsertLengthOffset[insertCode] + BitReader.ReadBits(br, Prefix.InsertLengthNBits[insertCode]);
				state.copyLength = Prefix.CopyLengthOffset[copyCode] + BitReader.ReadBits(br, Prefix.CopyLengthNBits[copyCode]);
				state.j = 0;
				state.runningState = 6;
				goto case 6;
			}
			case 6:
				if (state.trivialLiteralContext)
				{
					while (state.j < state.insertLength)
					{
						BitReader.ReadMoreInput(br);
						if (state.blockLength[0] == 0)
						{
							DecodeLiteralBlockSwitch(state);
						}
						state.blockLength[0]--;
						BitReader.FillBitWindow(br);
						ringBuffer[state.pos] = (byte)ReadSymbol(state.hGroup0.codes, state.literalTree, br);
						state.j++;
						if (state.pos++ == ringBufferMask)
						{
							state.nextRunningState = 6;
							state.bytesToWrite = state.ringBufferSize;
							state.bytesWritten = 0;
							state.runningState = 12;
							break;
						}
					}
				}
				else
				{
					int prevByte1 = ringBuffer[(state.pos - 1) & ringBufferMask] & 0xFF;
					int prevByte2 = ringBuffer[(state.pos - 2) & ringBufferMask] & 0xFF;
					while (state.j < state.insertLength)
					{
						BitReader.ReadMoreInput(br);
						if (state.blockLength[0] == 0)
						{
							DecodeLiteralBlockSwitch(state);
						}
						int literalTreeIndex = state.contextMap[state.contextMapSlice + (Context.Lookup[state.contextLookupOffset1 + prevByte1] | Context.Lookup[state.contextLookupOffset2 + prevByte2])] & 0xFF;
						state.blockLength[0]--;
						prevByte2 = prevByte1;
						BitReader.FillBitWindow(br);
						prevByte1 = ReadSymbol(state.hGroup0.codes, state.hGroup0.trees[literalTreeIndex], br);
						ringBuffer[state.pos] = (byte)prevByte1;
						state.j++;
						if (state.pos++ == ringBufferMask)
						{
							state.nextRunningState = 6;
							state.bytesToWrite = state.ringBufferSize;
							state.bytesWritten = 0;
							state.runningState = 12;
							break;
						}
					}
				}
				if (state.runningState != 6)
				{
					break;
				}
				state.metaBlockLength -= state.insertLength;
				if (state.metaBlockLength <= 0)
				{
					state.runningState = 3;
					break;
				}
				if (state.distanceCode < 0)
				{
					BitReader.ReadMoreInput(br);
					if (state.blockLength[2] == 0)
					{
						DecodeDistanceBlockSwitch(state);
					}
					state.blockLength[2]--;
					BitReader.FillBitWindow(br);
					state.distanceCode = ReadSymbol(state.hGroup2.codes, state.hGroup2.trees[state.distContextMap[state.distContextMapSlice + ((state.copyLength > 4) ? 3 : (state.copyLength - 2))] & 0xFF], br);
					if (state.distanceCode >= state.numDirectDistanceCodes)
					{
						state.distanceCode -= state.numDirectDistanceCodes;
						int postfix = state.distanceCode & state.distancePostfixMask;
						state.distanceCode >>>= state.distancePostfixBits;
						int j = (state.distanceCode >>> 1) + 1;
						int offset2 = (2 + (state.distanceCode & 1) << j) - 4;
						state.distanceCode = state.numDirectDistanceCodes + postfix + (offset2 + BitReader.ReadBits(br, j) << state.distancePostfixBits);
					}
				}
				state.distance = TranslateShortCodes(state.distanceCode, state.distRb, state.distRbIdx);
				if (state.distance < 0)
				{
					throw new BrotliRuntimeException("Negative distance");
				}
				if (state.maxDistance != state.maxBackwardDistance && state.pos < state.maxBackwardDistance)
				{
					state.maxDistance = state.pos;
				}
				else
				{
					state.maxDistance = state.maxBackwardDistance;
				}
				state.copyDst = state.pos;
				if (state.distance > state.maxDistance)
				{
					state.runningState = 9;
					break;
				}
				if (state.distanceCode > 0)
				{
					state.distRb[state.distRbIdx & 3] = state.distance;
					state.distRbIdx++;
				}
				if (state.copyLength > state.metaBlockLength)
				{
					throw new BrotliRuntimeException("Invalid backward reference");
				}
				state.j = 0;
				state.runningState = 7;
				goto case 7;
			case 7:
			{
				int src = (state.pos - state.distance) & ringBufferMask;
				int dst = state.pos;
				int copyLength = state.copyLength - state.j;
				if (src + copyLength < ringBufferMask && dst + copyLength < ringBufferMask)
				{
					for (int i = 0; i < copyLength; i++)
					{
						ringBuffer[dst++] = ringBuffer[src++];
					}
					state.j += copyLength;
					state.metaBlockLength -= copyLength;
					state.pos += copyLength;
				}
				else
				{
					while (state.j < state.copyLength)
					{
						ringBuffer[state.pos] = ringBuffer[(state.pos - state.distance) & ringBufferMask];
						state.metaBlockLength--;
						state.j++;
						if (state.pos++ == ringBufferMask)
						{
							state.nextRunningState = 7;
							state.bytesToWrite = state.ringBufferSize;
							state.bytesWritten = 0;
							state.runningState = 12;
							break;
						}
					}
				}
				if (state.runningState == 7)
				{
					state.runningState = 3;
				}
				break;
			}
			case 9:
				if (state.copyLength >= 4 && state.copyLength <= 24)
				{
					int offset = Dictionary.OffsetsByLength[state.copyLength];
					int num = state.distance - state.maxDistance - 1;
					int shift = Dictionary.SizeBitsByLength[state.copyLength];
					int mask = (1 << shift) - 1;
					int wordIdx = num & mask;
					int transformIdx = num >>> shift;
					offset += wordIdx * state.copyLength;
					if (transformIdx < Transform.Transforms.Length)
					{
						int len = Transform.TransformDictionaryWord(ringBuffer, state.copyDst, Dictionary.GetData(), offset, state.copyLength, Transform.Transforms[transformIdx]);
						state.copyDst += len;
						state.pos += len;
						state.metaBlockLength -= len;
						if (state.copyDst >= state.ringBufferSize)
						{
							state.nextRunningState = 8;
							state.bytesToWrite = state.ringBufferSize;
							state.bytesWritten = 0;
							state.runningState = 12;
						}
						else
						{
							state.runningState = 3;
						}
						break;
					}
					throw new BrotliRuntimeException("Invalid backward reference");
				}
				throw new BrotliRuntimeException("Invalid backward reference");
			case 8:
				Array.Copy(ringBuffer, state.ringBufferSize, ringBuffer, 0, state.copyDst - state.ringBufferSize);
				state.runningState = 3;
				break;
			case 4:
				while (state.metaBlockLength > 0)
				{
					BitReader.ReadMoreInput(br);
					BitReader.ReadBits(br, 8);
					state.metaBlockLength--;
				}
				state.runningState = 1;
				break;
			case 5:
				CopyUncompressedData(state);
				break;
			case 12:
				if (!WriteRingBuffer(state))
				{
					return;
				}
				if (state.pos >= state.maxBackwardDistance)
				{
					state.maxDistance = state.maxBackwardDistance;
				}
				state.pos &= ringBufferMask;
				state.runningState = state.nextRunningState;
				break;
			default:
				throw new BrotliRuntimeException("Unexpected state " + state.runningState);
			}
		}
		if (state.runningState == 10)
		{
			if (state.metaBlockLength < 0)
			{
				throw new BrotliRuntimeException("Invalid metablock length");
			}
			BitReader.JumpToByteBoundary(br);
			BitReader.CheckHealth(state.br, endOfStream: true);
		}
	}
}
