using System.Collections.Generic;

namespace SpirV;

public class FunctionType : Type
{
	public Type ReturnType { get; }

	public IReadOnlyList<Type> ParameterTypes { get; }

	public FunctionType(Type returnType, IReadOnlyList<Type> parameterTypes)
	{
		ReturnType = returnType;
		ParameterTypes = parameterTypes;
	}
}
