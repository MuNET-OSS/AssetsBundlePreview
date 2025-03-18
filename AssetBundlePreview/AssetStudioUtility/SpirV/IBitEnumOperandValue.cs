using System.Collections.Generic;

namespace SpirV;

public interface IBitEnumOperandValue : IEnumOperandValue
{
	IReadOnlyDictionary<uint, IReadOnlyList<object>> Values { get; }
}
