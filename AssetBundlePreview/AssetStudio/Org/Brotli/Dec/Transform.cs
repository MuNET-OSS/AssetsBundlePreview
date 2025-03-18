namespace Org.Brotli.Dec;

internal sealed class Transform
{
	private readonly byte[] prefix;

	private readonly int type;

	private readonly byte[] suffix;

	internal static readonly Transform[] Transforms = new Transform[121]
	{
		new Transform(string.Empty, 0, string.Empty),
		new Transform(string.Empty, 0, " "),
		new Transform(" ", 0, " "),
		new Transform(string.Empty, 12, string.Empty),
		new Transform(string.Empty, 10, " "),
		new Transform(string.Empty, 0, " the "),
		new Transform(" ", 0, string.Empty),
		new Transform("s ", 0, " "),
		new Transform(string.Empty, 0, " of "),
		new Transform(string.Empty, 10, string.Empty),
		new Transform(string.Empty, 0, " and "),
		new Transform(string.Empty, 13, string.Empty),
		new Transform(string.Empty, 1, string.Empty),
		new Transform(", ", 0, " "),
		new Transform(string.Empty, 0, ", "),
		new Transform(" ", 10, " "),
		new Transform(string.Empty, 0, " in "),
		new Transform(string.Empty, 0, " to "),
		new Transform("e ", 0, " "),
		new Transform(string.Empty, 0, "\""),
		new Transform(string.Empty, 0, "."),
		new Transform(string.Empty, 0, "\">"),
		new Transform(string.Empty, 0, "\n"),
		new Transform(string.Empty, 3, string.Empty),
		new Transform(string.Empty, 0, "]"),
		new Transform(string.Empty, 0, " for "),
		new Transform(string.Empty, 14, string.Empty),
		new Transform(string.Empty, 2, string.Empty),
		new Transform(string.Empty, 0, " a "),
		new Transform(string.Empty, 0, " that "),
		new Transform(" ", 10, string.Empty),
		new Transform(string.Empty, 0, ". "),
		new Transform(".", 0, string.Empty),
		new Transform(" ", 0, ", "),
		new Transform(string.Empty, 15, string.Empty),
		new Transform(string.Empty, 0, " with "),
		new Transform(string.Empty, 0, "'"),
		new Transform(string.Empty, 0, " from "),
		new Transform(string.Empty, 0, " by "),
		new Transform(string.Empty, 16, string.Empty),
		new Transform(string.Empty, 17, string.Empty),
		new Transform(" the ", 0, string.Empty),
		new Transform(string.Empty, 4, string.Empty),
		new Transform(string.Empty, 0, ". The "),
		new Transform(string.Empty, 11, string.Empty),
		new Transform(string.Empty, 0, " on "),
		new Transform(string.Empty, 0, " as "),
		new Transform(string.Empty, 0, " is "),
		new Transform(string.Empty, 7, string.Empty),
		new Transform(string.Empty, 1, "ing "),
		new Transform(string.Empty, 0, "\n\t"),
		new Transform(string.Empty, 0, ":"),
		new Transform(" ", 0, ". "),
		new Transform(string.Empty, 0, "ed "),
		new Transform(string.Empty, 20, string.Empty),
		new Transform(string.Empty, 18, string.Empty),
		new Transform(string.Empty, 6, string.Empty),
		new Transform(string.Empty, 0, "("),
		new Transform(string.Empty, 10, ", "),
		new Transform(string.Empty, 8, string.Empty),
		new Transform(string.Empty, 0, " at "),
		new Transform(string.Empty, 0, "ly "),
		new Transform(" the ", 0, " of "),
		new Transform(string.Empty, 5, string.Empty),
		new Transform(string.Empty, 9, string.Empty),
		new Transform(" ", 10, ", "),
		new Transform(string.Empty, 10, "\""),
		new Transform(".", 0, "("),
		new Transform(string.Empty, 11, " "),
		new Transform(string.Empty, 10, "\">"),
		new Transform(string.Empty, 0, "=\""),
		new Transform(" ", 0, "."),
		new Transform(".com/", 0, string.Empty),
		new Transform(" the ", 0, " of the "),
		new Transform(string.Empty, 10, "'"),
		new Transform(string.Empty, 0, ". This "),
		new Transform(string.Empty, 0, ","),
		new Transform(".", 0, " "),
		new Transform(string.Empty, 10, "("),
		new Transform(string.Empty, 10, "."),
		new Transform(string.Empty, 0, " not "),
		new Transform(" ", 0, "=\""),
		new Transform(string.Empty, 0, "er "),
		new Transform(" ", 11, " "),
		new Transform(string.Empty, 0, "al "),
		new Transform(" ", 11, string.Empty),
		new Transform(string.Empty, 0, "='"),
		new Transform(string.Empty, 11, "\""),
		new Transform(string.Empty, 10, ". "),
		new Transform(" ", 0, "("),
		new Transform(string.Empty, 0, "ful "),
		new Transform(" ", 10, ". "),
		new Transform(string.Empty, 0, "ive "),
		new Transform(string.Empty, 0, "less "),
		new Transform(string.Empty, 11, "'"),
		new Transform(string.Empty, 0, "est "),
		new Transform(" ", 10, "."),
		new Transform(string.Empty, 11, "\">"),
		new Transform(" ", 0, "='"),
		new Transform(string.Empty, 10, ","),
		new Transform(string.Empty, 0, "ize "),
		new Transform(string.Empty, 11, "."),
		new Transform("Â\u00a0", 0, string.Empty),
		new Transform(" ", 0, ","),
		new Transform(string.Empty, 10, "=\""),
		new Transform(string.Empty, 11, "=\""),
		new Transform(string.Empty, 0, "ous "),
		new Transform(string.Empty, 11, ", "),
		new Transform(string.Empty, 10, "='"),
		new Transform(" ", 10, ","),
		new Transform(" ", 11, "=\""),
		new Transform(" ", 11, ", "),
		new Transform(string.Empty, 11, ","),
		new Transform(string.Empty, 11, "("),
		new Transform(string.Empty, 11, ". "),
		new Transform(" ", 11, "."),
		new Transform(string.Empty, 11, "='"),
		new Transform(" ", 11, ". "),
		new Transform(" ", 10, "=\""),
		new Transform(" ", 11, "='"),
		new Transform(" ", 10, "='")
	};

	internal Transform(string prefix, int type, string suffix)
	{
		this.prefix = ReadUniBytes(prefix);
		this.type = type;
		this.suffix = ReadUniBytes(suffix);
	}

	internal static byte[] ReadUniBytes(string uniBytes)
	{
		byte[] result = new byte[uniBytes.Length];
		for (int i = 0; i < result.Length; i++)
		{
			result[i] = (byte)uniBytes[i];
		}
		return result;
	}

	internal static int TransformDictionaryWord(byte[] dst, int dstOffset, byte[] word, int wordOffset, int len, Transform transform)
	{
		int offset = dstOffset;
		byte[] @string = transform.prefix;
		int tmp = @string.Length;
		int i = 0;
		while (i < tmp)
		{
			dst[offset++] = @string[i++];
		}
		int op = transform.type;
		tmp = WordTransformType.GetOmitFirst(op);
		if (tmp > len)
		{
			tmp = len;
		}
		wordOffset += tmp;
		len -= tmp;
		len -= WordTransformType.GetOmitLast(op);
		for (i = len; i > 0; i--)
		{
			dst[offset++] = word[wordOffset++];
		}
		if (op == 11 || op == 10)
		{
			int uppercaseOffset = offset - len;
			if (op == 10)
			{
				len = 1;
			}
			while (len > 0)
			{
				tmp = dst[uppercaseOffset] & 0xFF;
				if (tmp < 192)
				{
					if (tmp >= 97 && tmp <= 122)
					{
						dst[uppercaseOffset] ^= 32;
					}
					uppercaseOffset++;
					len--;
				}
				else if (tmp < 224)
				{
					dst[uppercaseOffset + 1] ^= 32;
					uppercaseOffset += 2;
					len -= 2;
				}
				else
				{
					dst[uppercaseOffset + 2] ^= 5;
					uppercaseOffset += 3;
					len -= 3;
				}
			}
		}
		@string = transform.suffix;
		tmp = @string.Length;
		i = 0;
		while (i < tmp)
		{
			dst[offset++] = @string[i++];
		}
		return offset - dstOffset;
	}
}
