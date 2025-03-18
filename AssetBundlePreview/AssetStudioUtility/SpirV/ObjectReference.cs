using System.Collections.Generic;
using System.Text;

namespace SpirV;

public class ObjectReference
{
	public uint Id { get; }

	public ParsedInstruction Reference { get; private set; }

	public ObjectReference(uint id)
	{
		Id = id;
	}

	public void Resolve(IReadOnlyDictionary<uint, ParsedInstruction> objects)
	{
		Reference = objects[Id];
	}

	public override string ToString()
	{
		return $"%{Id}";
	}

	public StringBuilder ToString(StringBuilder sb)
	{
		return sb.Append('%').Append(Id);
	}
}
