namespace SpirV;

public class Operand
{
	public string Name { get; }

	public OperandType Type { get; }

	public OperandQuantifier Quantifier { get; }

	public Operand(OperandType kind, string name, OperandQuantifier quantifier)
	{
		Name = name;
		Type = kind;
		Quantifier = quantifier;
	}
}
