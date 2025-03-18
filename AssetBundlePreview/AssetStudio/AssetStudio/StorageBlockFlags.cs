using System;

namespace AssetStudio;

[Flags]
public enum StorageBlockFlags
{
	CompressionTypeMask = 0x3F,
	Streamed = 0x40
}
