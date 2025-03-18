using System;

namespace SpirV;

[Flags]
public enum KernelProfilingInfo : uint
{
	None = 0u,
	CmdExecTime = 1u
}
