using System;
using System.Collections.Generic;

namespace SpirV;

public class ParsedOperand
{
	public IReadOnlyList<uint> Words { get; }

	public object Value { get; set; }

	public Operand Operand { get; }

	public ParsedOperand(IReadOnlyList<uint> words, int index, int count, object value, Operand operand)
	{
		uint[] array = new uint[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = words[index + i];
		}
		Words = array;
		Value = value;
		Operand = operand;
	}

	public T GetSingleEnumValue<T>() where T : Enum
	{
		IValueEnumOperandValue v = (IValueEnumOperandValue)Value;
		if (v.Value.Count == 0)
		{
			return (T)v.Key;
		}
		return (T)((IValueEnumOperandValue)Value).Value[0];
	}

	public uint GetId()
	{
		return ((ObjectReference)Value).Id;
	}

	public T GetBitEnumValue<T>() where T : Enum
	{
		IBitEnumOperandValue obj = Value as IBitEnumOperandValue;
		uint result = 0u;
		foreach (uint i in obj.Values.Keys)
		{
			result |= i;
		}
		return (T)(object)result;
	}
}
