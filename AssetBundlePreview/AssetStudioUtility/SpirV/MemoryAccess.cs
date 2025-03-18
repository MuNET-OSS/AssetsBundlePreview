using System;

namespace SpirV;

[Flags]
public enum MemoryAccess : uint
{
	None = 0u,
	Volatile = 1u,
	Aligned = 2u,
	Nontemporal = 4u
}
