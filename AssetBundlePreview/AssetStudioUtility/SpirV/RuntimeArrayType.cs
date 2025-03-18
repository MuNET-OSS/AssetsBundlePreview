namespace SpirV;

public class RuntimeArrayType : Type
{
	public Type ElementType { get; }

	public RuntimeArrayType(Type elementType)
	{
		ElementType = elementType;
	}
}
