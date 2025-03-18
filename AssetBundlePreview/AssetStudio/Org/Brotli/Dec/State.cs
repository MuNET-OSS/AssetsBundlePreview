using System;
using System.IO;

namespace Org.Brotli.Dec;

internal sealed class State
{
	internal int runningState;

	internal int nextRunningState;

	internal readonly BitReader br = new BitReader();

	internal byte[] ringBuffer;

	internal readonly int[] blockTypeTrees = new int[3240];

	internal readonly int[] blockLenTrees = new int[3240];

	internal int metaBlockLength;

	internal bool inputEnd;

	internal bool isUncompressed;

	internal bool isMetadata;

	internal readonly HuffmanTreeGroup hGroup0 = new HuffmanTreeGroup();

	internal readonly HuffmanTreeGroup hGroup1 = new HuffmanTreeGroup();

	internal readonly HuffmanTreeGroup hGroup2 = new HuffmanTreeGroup();

	internal readonly int[] blockLength = new int[3];

	internal readonly int[] numBlockTypes = new int[3];

	internal readonly int[] blockTypeRb = new int[6];

	internal readonly int[] distRb = new int[4] { 16, 15, 11, 4 };

	internal int pos;

	internal int maxDistance;

	internal int distRbIdx;

	internal bool trivialLiteralContext;

	internal int literalTreeIndex;

	internal int literalTree;

	internal int j;

	internal int insertLength;

	internal byte[] contextModes;

	internal byte[] contextMap;

	internal int contextMapSlice;

	internal int distContextMapSlice;

	internal int contextLookupOffset1;

	internal int contextLookupOffset2;

	internal int treeCommandOffset;

	internal int distanceCode;

	internal byte[] distContextMap;

	internal int numDirectDistanceCodes;

	internal int distancePostfixMask;

	internal int distancePostfixBits;

	internal int distance;

	internal int copyLength;

	internal int copyDst;

	internal int maxBackwardDistance;

	internal int maxRingBufferSize;

	internal int ringBufferSize;

	internal long expectedTotalSize;

	internal byte[] customDictionary = new byte[0];

	internal int bytesToIgnore;

	internal int outputOffset;

	internal int outputLength;

	internal int outputUsed;

	internal int bytesWritten;

	internal int bytesToWrite;

	internal byte[] output;

	private static int DecodeWindowBits(BitReader br)
	{
		if (BitReader.ReadBits(br, 1) == 0)
		{
			return 16;
		}
		int i = BitReader.ReadBits(br, 3);
		if (i != 0)
		{
			return 17 + i;
		}
		i = BitReader.ReadBits(br, 3);
		if (i != 0)
		{
			return 8 + i;
		}
		return 17;
	}

	internal static void SetInput(State state, Stream input)
	{
		if (state.runningState != 0)
		{
			throw new InvalidOperationException("State MUST be uninitialized");
		}
		BitReader.Init(state.br, input);
		int windowBits = DecodeWindowBits(state.br);
		if (windowBits == 9)
		{
			throw new BrotliRuntimeException("Invalid 'windowBits' code");
		}
		state.maxRingBufferSize = 1 << windowBits;
		state.maxBackwardDistance = state.maxRingBufferSize - 16;
		state.runningState = 1;
	}

	internal static void Close(State state)
	{
		if (state.runningState == 0)
		{
			throw new InvalidOperationException("State MUST be initialized");
		}
		if (state.runningState != 11)
		{
			state.runningState = 11;
			BitReader.Close(state.br);
		}
	}
}
