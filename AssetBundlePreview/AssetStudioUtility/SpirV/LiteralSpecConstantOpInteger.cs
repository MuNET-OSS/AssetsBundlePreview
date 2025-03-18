using System.Collections.Generic;

namespace SpirV;

public class LiteralSpecConstantOpInteger : Literal
{
	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		List<ObjectReference> result = new List<ObjectReference>();
		for (int i = index; i < words.Count; i++)
		{
			ObjectReference objRef = new ObjectReference(words[i]);
			result.Add(objRef);
		}
		value = result;
		wordsUsed = words.Count - index;
		return true;
	}
}
