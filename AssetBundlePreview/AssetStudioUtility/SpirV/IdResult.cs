using System.Collections.Generic;

namespace SpirV;

public class IdResult : IdType
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		value = new ObjectReference(words[index]);
		wordsUsed = 1;
		return true;
	}
}
