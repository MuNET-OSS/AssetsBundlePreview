using System.Collections.Generic;
using System.Text;

namespace SpirV;

public class LiteralString : Literal
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		wordsUsed = 1;
		int bytesUsed = 0;
		byte[] bytes = new byte[(words.Count - index) * 4];
		for (int i = index; i < words.Count; i++)
		{
			uint word = words[i];
			byte b0 = (byte)(word & 0xFFu);
			if (b0 == 0)
			{
				break;
			}
			bytes[bytesUsed++] = b0;
			byte b1 = (byte)((word >> 8) & 0xFFu);
			if (b1 == 0)
			{
				break;
			}
			bytes[bytesUsed++] = b1;
			byte b2 = (byte)((word >> 16) & 0xFFu);
			if (b2 == 0)
			{
				break;
			}
			bytes[bytesUsed++] = b2;
			byte b3 = (byte)(word >> 24);
			if (b3 == 0)
			{
				break;
			}
			bytes[bytesUsed++] = b3;
			wordsUsed++;
		}
		value = Encoding.UTF8.GetString(bytes, 0, bytesUsed);
		return true;
	}
}
