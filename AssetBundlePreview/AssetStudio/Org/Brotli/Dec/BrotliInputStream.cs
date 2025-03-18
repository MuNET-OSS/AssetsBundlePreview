using System;
using System.IO;

namespace Org.Brotli.Dec;

public class BrotliInputStream : Stream
{
	public const int DefaultInternalBufferSize = 16384;

	private byte[] buffer;

	private int remainingBufferBytes;

	private int bufferOffset;

	private readonly State state = new State();

	public override bool CanRead => true;

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
	{
		get
		{
			throw new NotSupportedException();
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public override bool CanWrite => false;

	public BrotliInputStream(Stream source)
		: this(source, 16384, null)
	{
	}

	public BrotliInputStream(Stream source, int byteReadBufferSize)
		: this(source, byteReadBufferSize, null)
	{
	}

	public BrotliInputStream(Stream source, int byteReadBufferSize, byte[] customDictionary)
	{
		if (byteReadBufferSize <= 0)
		{
			throw new ArgumentException("Bad buffer size:" + byteReadBufferSize);
		}
		if (source == null)
		{
			throw new ArgumentException("source is null");
		}
		buffer = new byte[byteReadBufferSize];
		remainingBufferBytes = 0;
		bufferOffset = 0;
		try
		{
			State.SetInput(state, source);
		}
		catch (BrotliRuntimeException ex)
		{
			throw new IOException("Brotli decoder initialization failed", ex);
		}
		if (customDictionary != null)
		{
			Decode.SetCustomDictionary(state, customDictionary);
		}
	}

	public override void Close()
	{
		State.Close(state);
	}

	public override int ReadByte()
	{
		if (bufferOffset >= remainingBufferBytes)
		{
			remainingBufferBytes = Read(buffer, 0, buffer.Length);
			bufferOffset = 0;
			if (remainingBufferBytes == -1)
			{
				return -1;
			}
		}
		return buffer[bufferOffset++] & 0xFF;
	}

	public override int Read(byte[] destBuffer, int destOffset, int destLen)
	{
		if (destOffset < 0)
		{
			throw new ArgumentException("Bad offset: " + destOffset);
		}
		if (destLen < 0)
		{
			throw new ArgumentException("Bad length: " + destLen);
		}
		if (destOffset + destLen > destBuffer.Length)
		{
			throw new ArgumentException("Buffer overflow: " + (destOffset + destLen) + " > " + destBuffer.Length);
		}
		if (destLen == 0)
		{
			return 0;
		}
		int copyLen = Math.Max(remainingBufferBytes - bufferOffset, 0);
		if (copyLen != 0)
		{
			copyLen = Math.Min(copyLen, destLen);
			Array.Copy(buffer, bufferOffset, destBuffer, destOffset, copyLen);
			bufferOffset += copyLen;
			destOffset += copyLen;
			destLen -= copyLen;
			if (destLen == 0)
			{
				return copyLen;
			}
		}
		try
		{
			state.output = destBuffer;
			state.outputOffset = destOffset;
			state.outputLength = destLen;
			state.outputUsed = 0;
			Decode.Decompress(state);
			if (state.outputUsed == 0)
			{
				return 0;
			}
			return state.outputUsed + copyLen;
		}
		catch (BrotliRuntimeException ex)
		{
			throw new IOException("Brotli stream decoding failed", ex);
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override void Flush()
	{
	}
}
