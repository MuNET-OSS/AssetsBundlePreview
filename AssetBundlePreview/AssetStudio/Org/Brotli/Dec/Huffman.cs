namespace Org.Brotli.Dec;

internal sealed class Huffman
{
	internal const int HuffmanMaxTableSize = 1080;

	private const int MaxLength = 15;

	private static int GetNextKey(int key, int len)
	{
		int step = 1 << len - 1;
		while ((key & step) != 0)
		{
			step >>= 1;
		}
		return (key & (step - 1)) + step;
	}

	private static void ReplicateValue(int[] table, int offset, int step, int end, int item)
	{
		do
		{
			end -= step;
			table[offset + end] = item;
		}
		while (end > 0);
	}

	private static int NextTableBitSize(int[] count, int len, int rootBits)
	{
		int left = 1 << len - rootBits;
		while (len < 15)
		{
			left -= count[len];
			if (left <= 0)
			{
				break;
			}
			len++;
			left <<= 1;
		}
		return len - rootBits;
	}

	internal static void BuildHuffmanTable(int[] rootTable, int tableOffset, int rootBits, int[] codeLengths, int codeLengthsSize)
	{
		int[] sorted = new int[codeLengthsSize];
		int[] count = new int[16];
		int[] offset = new int[16];
		int symbol;
		for (symbol = 0; symbol < codeLengthsSize; symbol++)
		{
			count[codeLengths[symbol]]++;
		}
		offset[1] = 0;
		for (int len3 = 1; len3 < 15; len3++)
		{
			offset[len3 + 1] = offset[len3] + count[len3];
		}
		for (symbol = 0; symbol < codeLengthsSize; symbol++)
		{
			if (codeLengths[symbol] != 0)
			{
				sorted[offset[codeLengths[symbol]]++] = symbol;
			}
		}
		int tableBits = rootBits;
		int tableSize = 1 << tableBits;
		int totalSize = tableSize;
		int key;
		if (offset[15] == 1)
		{
			for (key = 0; key < totalSize; key++)
			{
				rootTable[tableOffset + key] = sorted[0];
			}
			return;
		}
		key = 0;
		symbol = 0;
		int len2 = 1;
		int step2 = 2;
		while (len2 <= rootBits)
		{
			while (count[len2] > 0)
			{
				ReplicateValue(rootTable, tableOffset + key, step2, tableSize, (len2 << 16) | sorted[symbol++]);
				key = GetNextKey(key, len2);
				count[len2]--;
			}
			len2++;
			step2 <<= 1;
		}
		int mask = totalSize - 1;
		int low = -1;
		int currentOffset = tableOffset;
		int len = rootBits + 1;
		int step = 2;
		while (len <= 15)
		{
			while (count[len] > 0)
			{
				if ((key & mask) != low)
				{
					currentOffset += tableSize;
					tableBits = NextTableBitSize(count, len, rootBits);
					tableSize = 1 << tableBits;
					totalSize += tableSize;
					low = key & mask;
					rootTable[tableOffset + low] = (tableBits + rootBits << 16) | (currentOffset - tableOffset - low);
				}
				ReplicateValue(rootTable, currentOffset + (key >> rootBits), step, tableSize, (len - rootBits << 16) | sorted[symbol++]);
				key = GetNextKey(key, len);
				count[len]--;
			}
			len++;
			step <<= 1;
		}
	}
}
