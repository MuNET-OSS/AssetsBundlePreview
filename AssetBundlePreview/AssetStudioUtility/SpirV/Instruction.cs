using System.Collections.Generic;

namespace SpirV;

public class Instruction
{
	public string Name { get; }

	public IReadOnlyList<Operand> Operands { get; }

	public Instruction(string name)
		: this(name, new List<Operand>())
	{
	}

	public Instruction(string name, IReadOnlyList<Operand> operands)
	{
		Operands = operands;
		Name = name;
	}
}
