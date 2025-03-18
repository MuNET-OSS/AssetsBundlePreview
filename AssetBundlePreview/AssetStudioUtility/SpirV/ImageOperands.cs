using System;

namespace SpirV;

[Flags]
public enum ImageOperands : uint
{
	None = 0u,
	Bias = 1u,
	Lod = 2u,
	Grad = 4u,
	ConstOffset = 8u,
	Offset = 0x10u,
	ConstOffsets = 0x20u,
	Sample = 0x40u,
	MinLod = 0x80u
}
