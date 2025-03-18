using System;

namespace SpirV;

public struct ModuleHeader
{
	public Version Version { get; set; }

	public string GeneratorVendor { get; set; }

	public string GeneratorName { get; set; }

	public int GeneratorVersion { get; set; }

	public uint Bound { get; set; }

	public uint Reserved { get; set; }
}
