using System.Collections.Generic;

namespace SpirV;

public class PairIdRefIdRef : OperandType
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		ObjectReference variable = new ObjectReference(words[index]);
		ObjectReference parent = new ObjectReference(words[index + 1]);
		value = new
		{
			Variable = variable,
			Parent = parent
		};
		wordsUsed = 2;
		return true;
	}
}
