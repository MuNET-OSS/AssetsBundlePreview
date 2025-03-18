using System;

namespace SpirV;

[Flags]
public enum FPFastMathMode : uint
{
	None = 0u,
	NotNaN = 1u,
	NotInf = 2u,
	NSZ = 4u,
	AllowRecip = 8u,
	Fast = 0x10u
}
