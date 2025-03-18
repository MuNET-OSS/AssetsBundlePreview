using System.Collections.Generic;

namespace SpirV;

public class OperandType
{
	public virtual bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		value = GetType();
		wordsUsed = 1;
		return true;
	}
}
