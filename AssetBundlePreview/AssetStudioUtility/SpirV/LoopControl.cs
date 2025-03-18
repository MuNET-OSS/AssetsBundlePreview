using System;

namespace SpirV;

[Flags]
public enum LoopControl : uint
{
	None = 0u,
	Unroll = 1u,
	DontUnroll = 2u,
	DependencyInfinite = 4u,
	DependencyLength = 8u
}
