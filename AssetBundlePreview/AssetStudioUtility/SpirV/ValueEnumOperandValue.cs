using System;
using System.Collections.Generic;

namespace SpirV;

public class ValueEnumOperandValue<T> : IValueEnumOperandValue, IEnumOperandValue where T : Enum
{
	public System.Type EnumerationType => typeof(T);

	public object Key { get; }

	public IReadOnlyList<object> Value { get; }

	public ValueEnumOperandValue(T key, IReadOnlyList<object> value)
	{
		Key = key;
		Value = value;
	}
}
