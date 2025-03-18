namespace SevenZip.Compression.RangeCoder;

internal struct BitTreeEncoder
{
	private BitEncoder[] Models;

	private int NumBitLevels;

	public BitTreeEncoder(int numBitLevels)
	{
		NumBitLevels = numBitLevels;
		Models = new BitEncoder[1 << numBitLevels];
	}

	public void Init()
	{
		for (uint i = 1u; i < 1 << NumBitLevels; i++)
		{
			Models[i].Init();
		}
	}

	public void Encode(Encoder rangeEncoder, uint symbol)
	{
		uint i = 1u;
		int bitIndex = NumBitLevels;
		while (bitIndex > 0)
		{
			bitIndex--;
			uint bit = (symbol >> bitIndex) & 1u;
			Models[i].Encode(rangeEncoder, bit);
			i = (i << 1) | bit;
		}
	}

	public void ReverseEncode(Encoder rangeEncoder, uint symbol)
	{
		uint j = 1u;
		for (uint i = 0u; i < NumBitLevels; i++)
		{
			uint bit = symbol & 1u;
			Models[j].Encode(rangeEncoder, bit);
			j = (j << 1) | bit;
			symbol >>= 1;
		}
	}

	public uint GetPrice(uint symbol)
	{
		uint price = 0u;
		uint i = 1u;
		int bitIndex = NumBitLevels;
		while (bitIndex > 0)
		{
			bitIndex--;
			uint bit = (symbol >> bitIndex) & 1u;
			price += Models[i].GetPrice(bit);
			i = (i << 1) + bit;
		}
		return price;
	}

	public uint ReverseGetPrice(uint symbol)
	{
		uint price = 0u;
		uint j = 1u;
		for (int i = NumBitLevels; i > 0; i--)
		{
			uint bit = symbol & 1u;
			symbol >>= 1;
			price += Models[j].GetPrice(bit);
			j = (j << 1) | bit;
		}
		return price;
	}

	public static uint ReverseGetPrice(BitEncoder[] Models, uint startIndex, int NumBitLevels, uint symbol)
	{
		uint price = 0u;
		uint j = 1u;
		for (int i = NumBitLevels; i > 0; i--)
		{
			uint bit = symbol & 1u;
			symbol >>= 1;
			price += Models[startIndex + j].GetPrice(bit);
			j = (j << 1) | bit;
		}
		return price;
	}

	public static void ReverseEncode(BitEncoder[] Models, uint startIndex, Encoder rangeEncoder, int NumBitLevels, uint symbol)
	{
		uint j = 1u;
		for (int i = 0; i < NumBitLevels; i++)
		{
			uint bit = symbol & 1u;
			Models[startIndex + j].Encode(rangeEncoder, bit);
			j = (j << 1) | bit;
			symbol >>= 1;
		}
	}
}
