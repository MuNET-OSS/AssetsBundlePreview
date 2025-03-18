using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpirV;

public class EnumType<T> : EnumType<T, ParameterFactory> where T : Enum
{
}
public class EnumType<T, U> : OperandType where T : Enum where U : ParameterFactory, new()
{
	private U parameterFactory_ = new U();

	public System.Type EnumerationType => typeof(T);

	public override bool ReadValue(IReadOnlyList<uint> words, int index, out object value, out int wordsUsed)
	{
		int wordsUsedForParameters = 0;
		if (typeof(T).GetTypeInfo().GetCustomAttributes<FlagsAttribute>().Any())
		{
			Dictionary<uint, IReadOnlyList<object>> result = new Dictionary<uint, IReadOnlyList<object>>();
			foreach (uint bit in EnumerationType.GetEnumValues())
			{
				if ((words[index] & bit) == 0 && (bit != 0 || words[index] != 0))
				{
					continue;
				}
				Parameter p = parameterFactory_.CreateParameter(bit);
				if (p == null)
				{
					result.Add(bit, Array.Empty<object>());
					continue;
				}
				object[] resultItems = new object[p.OperandTypes.Count];
				for (int i = 0; i < p.OperandTypes.Count; i++)
				{
					p.OperandTypes[i].ReadValue(words, 1 + wordsUsedForParameters, out var pValue, out var pWordsUsed);
					wordsUsedForParameters += pWordsUsed;
					resultItems[i] = pValue;
				}
				result.Add(bit, resultItems);
			}
			value = new BitEnumOperandValue<T>(result);
		}
		else
		{
			Parameter p2 = parameterFactory_.CreateParameter(words[index]);
			object[] resultItems2;
			if (p2 == null)
			{
				resultItems2 = Array.Empty<object>();
			}
			else
			{
				resultItems2 = new object[p2.OperandTypes.Count];
				for (int j = 0; j < p2.OperandTypes.Count; j++)
				{
					p2.OperandTypes[j].ReadValue(words, 1 + wordsUsedForParameters, out var pValue2, out var pWordsUsed2);
					wordsUsedForParameters += pWordsUsed2;
					resultItems2[j] = pValue2;
				}
			}
			value = new ValueEnumOperandValue<T>((T)(object)words[index], resultItems2);
		}
		wordsUsed = wordsUsedForParameters + 1;
		return true;
	}
}
