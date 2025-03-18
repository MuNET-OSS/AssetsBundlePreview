using System;

namespace Org.Brotli.Dec;

[Serializable]
internal class BrotliRuntimeException : Exception
{
	internal BrotliRuntimeException(string message)
		: base(message)
	{
	}

	internal BrotliRuntimeException(string message, Exception cause)
		: base(message, cause)
	{
	}
}
