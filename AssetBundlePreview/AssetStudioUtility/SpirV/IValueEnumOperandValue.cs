using System.Collections.Generic;

namespace SpirV;

public interface IValueEnumOperandValue : IEnumOperandValue
{
	object Key { get; }

	IReadOnlyList<object> Value { get; }
}
