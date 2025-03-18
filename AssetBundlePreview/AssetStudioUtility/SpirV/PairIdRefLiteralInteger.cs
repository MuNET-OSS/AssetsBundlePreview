using System.Collections.Generic;

namespace SpirV;

public class PairIdRefLiteralInteger : OperandType
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		ObjectReference type = new ObjectReference(words[index]);
		uint word = words[index + 1];
		value = new
		{
			Type = type,
			Member = word
		};
		wordsUsed = 2;
		return true;
	}
}
