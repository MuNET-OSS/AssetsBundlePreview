using System.Collections.Generic;

namespace SpirV;

public class LiteralInteger : LiteralNumber
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		value = words[index];
		wordsUsed = 1;
		return true;
	}
}
