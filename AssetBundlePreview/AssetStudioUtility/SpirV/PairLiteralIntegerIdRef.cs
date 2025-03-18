using System.Collections.Generic;

namespace SpirV;

public class PairLiteralIntegerIdRef : OperandType
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		uint selector = words[index];
		ObjectReference label = new ObjectReference(words[index + 1]);
		value = new
		{
			Selector = selector,
			Label = label
		};
		wordsUsed = 2;
		return true;
	}
}
