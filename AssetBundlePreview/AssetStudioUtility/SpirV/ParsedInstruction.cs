using System;
using System.Collections.Generic;

namespace SpirV;

public class ParsedInstruction
{
	public Type ResultType { get; set; }

	public uint ResultId
	{
		get
		{
			for (int i = 0; i < Instruction.Operands.Count; i++)
			{
				if (Instruction.Operands[i].Type is IdResult)
				{
					return Operands[i].GetId();
				}
			}
			return 0u;
		}
	}

	public bool HasResult => ResultId != 0;

	public IReadOnlyList<uint> Words { get; }

	public Instruction Instruction { get; }

	public IList<ParsedOperand> Operands { get; } = new List<ParsedOperand>();


	public string Name { get; set; }

	public object Value { get; set; }

	public ParsedInstruction(int opCode, IReadOnlyList<uint> words)
	{
		Words = words;
		Instruction = Instructions.OpcodeToInstruction[opCode];
		ParseOperands();
	}

	private void ParseOperands()
	{
		if (Instruction.Operands.Count == 0)
		{
			return;
		}
		int currentWord = 1;
		int currentOperand = 0;
		List<object> varyingOperandValues = new List<object>();
		Operand varyingOperand = null;
		while (currentWord < Words.Count)
		{
			Operand operand = Instruction.Operands[currentOperand];
			operand.Type.ReadValue(Words, currentWord, out var value, out var wordsUsed);
			if (operand.Quantifier == OperandQuantifier.Varying)
			{
				varyingOperandValues.Add(value);
				varyingOperand = operand;
			}
			else
			{
				int wordCount = Math.Min(Words.Count - currentWord, wordsUsed);
				ParsedOperand parsedOperand2 = new ParsedOperand(Words, currentWord, wordCount, value, operand);
				Operands.Add(parsedOperand2);
			}
			currentWord += wordsUsed;
			if (operand.Quantifier != OperandQuantifier.Varying)
			{
				currentOperand++;
			}
		}
		if (varyingOperand != null)
		{
			VaryingOperandValue varOperantValue = new VaryingOperandValue(varyingOperandValues);
			ParsedOperand parsedOperand = new ParsedOperand(Words, currentWord, Words.Count - currentWord, varOperantValue, varyingOperand);
			Operands.Add(parsedOperand);
		}
	}

	public void ResolveResultType(IReadOnlyDictionary<uint, ParsedInstruction> objects)
	{
		if (Instruction.Operands.Count > 0 && Instruction.Operands[0].Type is IdResultType)
		{
			ResultType = objects[(uint)Operands[0].Value].ResultType;
		}
	}

	public void ResolveReferences(IReadOnlyDictionary<uint, ParsedInstruction> objects)
	{
		foreach (ParsedOperand operand in Operands)
		{
			if (operand.Value is ObjectReference objectReference)
			{
				objectReference.Resolve(objects);
			}
		}
	}
}
