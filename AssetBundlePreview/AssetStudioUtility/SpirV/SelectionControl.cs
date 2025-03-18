using System;

namespace SpirV;

[Flags]
public enum SelectionControl : uint
{
	None = 0u,
	Flatten = 1u,
	DontFlatten = 2u
}
