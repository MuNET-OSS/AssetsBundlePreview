namespace Org.Brotli.Dec;

internal sealed class HuffmanTreeGroup
{
	private int alphabetSize;

	internal int[] codes;

	internal int[] trees;

	internal static void Init(HuffmanTreeGroup group, int alphabetSize, int n)
	{
		group.alphabetSize = alphabetSize;
		group.codes = new int[n * 1080];
		group.trees = new int[n];
	}

	internal static void Decode(HuffmanTreeGroup group, BitReader br)
	{
		int next = 0;
		int j = group.trees.Length;
		for (int i = 0; i < j; i++)
		{
			group.trees[i] = next;
			Org.Brotli.Dec.Decode.ReadHuffmanCode(group.alphabetSize, group.codes, next, br);
			next += 1080;
		}
	}
}
