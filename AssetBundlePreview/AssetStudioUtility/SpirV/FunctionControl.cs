using System;

namespace SpirV;

[Flags]
public enum FunctionControl : uint
{
	None = 0u,
	Inline = 1u,
	DontInline = 2u,
	Pure = 4u,
	Const = 8u
}
