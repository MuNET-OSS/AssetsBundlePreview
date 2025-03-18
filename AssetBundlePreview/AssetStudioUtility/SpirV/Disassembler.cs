using System;
using System.Collections.Generic;
using System.Text;

namespace SpirV;

public class Disassembler
{
	private readonly StringBuilder m_sb = new StringBuilder();

	public string Disassemble(Module module)
	{
		return Disassemble(module, DisassemblyOptions.Default);
	}

	public string Disassemble(Module module, DisassemblyOptions options)
	{
		m_sb.AppendLine("; SPIR-V");
		m_sb.Append("; Version: ").Append(module.Header.Version).AppendLine();
		if (module.Header.GeneratorName == null)
		{
			m_sb.Append("; Generator: unknown; ").Append(module.Header.GeneratorVersion).AppendLine();
		}
		else
		{
			m_sb.Append("; Generator: ").Append(module.Header.GeneratorVendor).Append(' ')
				.Append(module.Header.GeneratorName)
				.Append("; ")
				.Append(module.Header.GeneratorVersion)
				.AppendLine();
		}
		m_sb.Append("; Bound: ").Append(module.Header.Bound).AppendLine();
		m_sb.Append("; Schema: ").Append(module.Header.Reserved).AppendLine();
		string[] lines = new string[module.Instructions.Count + 1];
		lines[0] = m_sb.ToString();
		m_sb.Clear();
		for (int k = 0; k < module.Instructions.Count; k++)
		{
			ParsedInstruction instruction = module.Instructions[k];
			PrintInstruction(m_sb, instruction, options);
			lines[k + 1] = m_sb.ToString();
			m_sb.Clear();
		}
		int longestPrefix = 0;
		foreach (string line2 in lines)
		{
			longestPrefix = Math.Max(longestPrefix, line2.IndexOf('='));
			if (longestPrefix > 50)
			{
				longestPrefix = 50;
				break;
			}
		}
		m_sb.Append(lines[0]);
		for (int i = 1; i < lines.Length; i++)
		{
			string line = lines[i];
			int index = line.IndexOf('=');
			if (index == -1)
			{
				m_sb.Append(' ', longestPrefix + 4);
				m_sb.Append(line);
			}
			else
			{
				int pad = Math.Max(0, longestPrefix - index);
				m_sb.Append(' ', pad);
				m_sb.Append(line, 0, index);
				m_sb.Append('=');
				m_sb.Append(line, index + 1, line.Length - index - 1);
			}
			m_sb.AppendLine();
		}
		string result = m_sb.ToString();
		m_sb.Clear();
		return result;
	}

	private static void PrintInstruction(StringBuilder sb, ParsedInstruction instruction, DisassemblyOptions options)
	{
		if (instruction.Operands.Count == 0)
		{
			sb.Append(instruction.Instruction.Name);
			return;
		}
		int currentOperand = 0;
		if (instruction.Instruction.Operands[currentOperand].Type is IdResultType)
		{
			if (options.HasFlag(DisassemblyOptions.ShowTypes))
			{
				instruction.ResultType.ToString(sb).Append(' ');
			}
			currentOperand++;
		}
		if (currentOperand < instruction.Operands.Count && instruction.Instruction.Operands[currentOperand].Type is IdResult)
		{
			if (!options.HasFlag(DisassemblyOptions.ShowNames) || string.IsNullOrWhiteSpace(instruction.Name))
			{
				PrintOperandValue(sb, instruction.Operands[currentOperand].Value, options);
			}
			else
			{
				sb.Append(instruction.Name);
			}
			sb.Append(" = ");
			currentOperand++;
		}
		sb.Append(instruction.Instruction.Name);
		sb.Append(' ');
		for (; currentOperand < instruction.Operands.Count; currentOperand++)
		{
			PrintOperandValue(sb, instruction.Operands[currentOperand].Value, options);
			sb.Append(' ');
		}
	}

	private static void PrintOperandValue(StringBuilder sb, object value, DisassemblyOptions options)
	{
		if (!(value is System.Type t))
		{
			if (!(value is string s))
			{
				if (!(value is ObjectReference or))
				{
					if (!(value is IBitEnumOperandValue beov))
					{
						if (!(value is IValueEnumOperandValue veov))
						{
							if (value is VaryingOperandValue varOpVal)
							{
								varOpVal.ToString(sb);
							}
							else
							{
								sb.Append(value);
							}
						}
						else
						{
							PrintValueEnumValue(sb, veov, options);
						}
					}
					else
					{
						PrintBitEnumValue(sb, beov, options);
					}
				}
				else if (options.HasFlag(DisassemblyOptions.ShowNames) && or.Reference != null && !string.IsNullOrWhiteSpace(or.Reference.Name))
				{
					sb.Append(or.Reference.Name);
				}
				else
				{
					or.ToString(sb);
				}
			}
			else
			{
				sb.Append('"');
				sb.Append(s);
				sb.Append('"');
			}
		}
		else
		{
			sb.Append(t.Name);
		}
	}

	private static void PrintBitEnumValue(StringBuilder sb, IBitEnumOperandValue enumOperandValue, DisassemblyOptions options)
	{
		foreach (uint key in enumOperandValue.Values.Keys)
		{
			sb.Append(enumOperandValue.EnumerationType.GetEnumName(key));
			IReadOnlyList<object> value = enumOperandValue.Values[key];
			if (value.Count == 0)
			{
				continue;
			}
			sb.Append(' ');
			foreach (object v in value)
			{
				PrintOperandValue(sb, v, options);
			}
		}
	}

	private static void PrintValueEnumValue(StringBuilder sb, IValueEnumOperandValue valueOperandValue, DisassemblyOptions options)
	{
		sb.Append(valueOperandValue.Key);
		if (!(valueOperandValue.Value is IList<object> { Count: >0 } valueList))
		{
			return;
		}
		sb.Append(' ');
		foreach (object v in valueList)
		{
			PrintOperandValue(sb, v, options);
		}
	}
}
