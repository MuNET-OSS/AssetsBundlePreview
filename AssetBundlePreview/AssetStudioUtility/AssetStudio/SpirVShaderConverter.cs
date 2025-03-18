using System;
using System.IO;
using System.Text;
using Smolv;
using SpirV;

namespace AssetStudio;

public static class SpirVShaderConverter
{
	public static string Convert(byte[] m_ProgramCode)
	{
		StringBuilder sb = new StringBuilder();
		using (MemoryStream ms = new MemoryStream(m_ProgramCode))
		{
			using BinaryReader reader = new BinaryReader(ms);
			reader.ReadInt32();
			int minOffset = m_ProgramCode.Length;
			int snippetCount = 5;
			for (int i = 0; i < snippetCount; i++)
			{
				if (reader.BaseStream.Position >= minOffset)
				{
					break;
				}
				int offset = reader.ReadInt32();
				int size = reader.ReadInt32();
				if (size > 0)
				{
					if (offset < minOffset)
					{
						minOffset = offset;
					}
					long pos = ms.Position;
					sb.Append(ExportSnippet(ms, offset, size));
					ms.Position = pos;
				}
			}
		}
		return sb.ToString();
	}

	private static string ExportSnippet(Stream stream, int offset, int size)
	{
		stream.Position = offset;
		int decodedBufferSize = SmolvDecoder.GetDecodedBufferSize(stream);
		if (decodedBufferSize == 0)
		{
			throw new Exception("Invalid SMOL-V shader header");
		}
		using MemoryStream decodedStream = new MemoryStream(new byte[decodedBufferSize]);
		if (SmolvDecoder.Decode(stream, size, decodedStream))
		{
			decodedStream.Position = 0L;
			Module module = Module.ReadFrom(decodedStream);
			return new Disassembler().Disassemble(module, DisassemblyOptions.Default).Replace("\r\n", "\n");
		}
		throw new Exception("Unable to decode SMOL-V shader");
	}
}
