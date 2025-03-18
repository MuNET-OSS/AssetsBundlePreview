namespace SevenZip.Compression.RangeCoder;

internal struct BitTreeDecoder
{
	private BitDecoder[] Models;

	private int NumBitLevels;

	public BitTreeDecoder(int numBitLevels)
	{
		NumBitLevels = numBitLevels;
		Models = new BitDecoder[1 << numBitLevels];
	}

	public void Init()
	{
		for (uint i = 1u; i < 1 << NumBitLevels; i++)
		{
			Models[i].Init();
		}
	}

	public uint Decode(Decoder rangeDecoder)
	{
		uint i = 1u;
		for (int bitIndex = NumBitLevels; bitIndex > 0; bitIndex--)
		{
			i = (i << 1) + Models[i].Decode(rangeDecoder);
		}
		return i - (uint)(1 << NumBitLevels);
	}

	public uint ReverseDecode(Decoder rangeDecoder)
	{
		uint i = 1u;
		uint symbol = 0u;
		for (int bitIndex = 0; bitIndex < NumBitLevels; bitIndex++)
		{
			uint bit = Models[i].Decode(rangeDecoder);
			i <<= 1;
			i += bit;
			symbol |= bit << bitIndex;
		}
		return symbol;
	}

	public static uint ReverseDecode(BitDecoder[] Models, uint startIndex, Decoder rangeDecoder, int NumBitLevels)
	{
		uint i = 1u;
		uint symbol = 0u;
		for (int bitIndex = 0; bitIndex < NumBitLevels; bitIndex++)
		{
			uint bit = Models[startIndex + i].Decode(rangeDecoder);
			i <<= 1;
			i += bit;
			symbol |= bit << bitIndex;
		}
		return symbol;
	}
}
