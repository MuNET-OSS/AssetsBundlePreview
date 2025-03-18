using System;

namespace SpirV;

[Flags]
public enum MemorySemantics : uint
{
	Relaxed = 0u,
	None = 0u,
	Acquire = 2u,
	Release = 4u,
	AcquireRelease = 8u,
	SequentiallyConsistent = 0x10u,
	UniformMemory = 0x40u,
	SubgroupMemory = 0x80u,
	WorkgroupMemory = 0x100u,
	CrossWorkgroupMemory = 0x200u,
	AtomicCounterMemory = 0x400u,
	ImageMemory = 0x800u
}
