using System.Collections.Generic;

namespace SpirV;

public class IdScope : OperandType
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		value = (Scope)words[index];
		wordsUsed = 1;
		return true;
	}
}
