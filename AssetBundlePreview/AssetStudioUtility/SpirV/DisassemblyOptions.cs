using System;

namespace SpirV;

[Flags]
public enum DisassemblyOptions
{
	None = 0,
	ShowTypes = 1,
	ShowNames = 2,
	Default = 3
}
