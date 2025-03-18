using System;
using System.Collections.Generic;

namespace SpirV;

public class BitEnumOperandValue<T> : IBitEnumOperandValue, IEnumOperandValue where T : Enum
{
	public IReadOnlyDictionary<uint, IReadOnlyList<object>> Values { get; }

	public System.Type EnumerationType => typeof(T);

	public BitEnumOperandValue(Dictionary<uint, IReadOnlyList<object>> values)
	{
		Values = values;
	}
}
