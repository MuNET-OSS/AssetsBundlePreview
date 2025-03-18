namespace Org.Brotli.Dec;

internal sealed class Prefix
{
	internal static readonly int[] BlockLengthOffset = new int[26]
	{
		1, 5, 9, 13, 17, 25, 33, 41, 49, 65,
		81, 97, 113, 145, 177, 209, 241, 305, 369, 497,
		753, 1265, 2289, 4337, 8433, 16625
	};

	internal static readonly int[] BlockLengthNBits = new int[26]
	{
		2, 2, 2, 2, 3, 3, 3, 3, 4, 4,
		4, 4, 5, 5, 5, 5, 6, 6, 7, 8,
		9, 10, 11, 12, 13, 24
	};

	internal static readonly int[] InsertLengthOffset = new int[24]
	{
		0, 1, 2, 3, 4, 5, 6, 8, 10, 14,
		18, 26, 34, 50, 66, 98, 130, 194, 322, 578,
		1090, 2114, 6210, 22594
	};

	internal static readonly int[] InsertLengthNBits = new int[24]
	{
		0, 0, 0, 0, 0, 0, 1, 1, 2, 2,
		3, 3, 4, 4, 5, 5, 6, 7, 8, 9,
		10, 12, 14, 24
	};

	internal static readonly int[] CopyLengthOffset = new int[24]
	{
		2, 3, 4, 5, 6, 7, 8, 9, 10, 12,
		14, 18, 22, 30, 38, 54, 70, 102, 134, 198,
		326, 582, 1094, 2118
	};

	internal static readonly int[] CopyLengthNBits = new int[24]
	{
		0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
		2, 2, 3, 3, 4, 4, 5, 5, 6, 7,
		8, 9, 10, 24
	};

	internal static readonly int[] InsertRangeLut = new int[9] { 0, 0, 8, 8, 0, 16, 8, 16, 16 };

	internal static readonly int[] CopyRangeLut = new int[9] { 0, 8, 0, 8, 16, 0, 16, 8, 16 };
}
