using System;
using System.IO;
using System.Text;

namespace AssetStudio;

public class ShaderSubProgram
{
	private int m_Version;

	public ShaderGpuProgramType m_ProgramType;

	public string[] m_Keywords;

	public string[] m_LocalKeywords;

	public byte[] m_ProgramCode;

	public ShaderSubProgram(BinaryReader reader)
	{
		m_Version = reader.ReadInt32();
		m_ProgramType = (ShaderGpuProgramType)reader.ReadInt32();
		reader.BaseStream.Position += 12L;
		if (m_Version >= 201608170)
		{
			reader.BaseStream.Position += 4L;
		}
		int m_KeywordsSize = reader.ReadInt32();
		m_Keywords = new string[m_KeywordsSize];
		for (int j = 0; j < m_KeywordsSize; j++)
		{
			m_Keywords[j] = reader.ReadAlignedString();
		}
		if (m_Version >= 201806140 && m_Version < 202012090)
		{
			int m_LocalKeywordsSize = reader.ReadInt32();
			m_LocalKeywords = new string[m_LocalKeywordsSize];
			for (int i = 0; i < m_LocalKeywordsSize; i++)
			{
				m_LocalKeywords[i] = reader.ReadAlignedString();
			}
		}
		m_ProgramCode = reader.ReadUInt8Array();
		reader.AlignStream();
	}

	public string Export()
	{
		StringBuilder sb = new StringBuilder();
		if (m_Keywords.Length != 0)
		{
			sb.Append("Keywords { ");
			string[] keywords = m_Keywords;
			foreach (string keyword2 in keywords)
			{
				sb.Append("\"" + keyword2 + "\" ");
			}
			sb.Append("}\n");
		}
		if (m_LocalKeywords != null && m_LocalKeywords.Length != 0)
		{
			sb.Append("Local Keywords { ");
			string[] keywords = m_LocalKeywords;
			foreach (string keyword in keywords)
			{
				sb.Append("\"" + keyword + "\" ");
			}
			sb.Append("}\n");
		}
		sb.Append("\"");
		if (m_ProgramCode.Length != 0)
		{
			switch (m_ProgramType)
			{
			case ShaderGpuProgramType.GLLegacy:
			case ShaderGpuProgramType.GLES31AEP:
			case ShaderGpuProgramType.GLES31:
			case ShaderGpuProgramType.GLES3:
			case ShaderGpuProgramType.GLES:
			case ShaderGpuProgramType.GLCore32:
			case ShaderGpuProgramType.GLCore41:
			case ShaderGpuProgramType.GLCore43:
				sb.Append(Encoding.UTF8.GetString(m_ProgramCode));
				break;
			case ShaderGpuProgramType.DX9VertexSM20:
			case ShaderGpuProgramType.DX9VertexSM30:
			case ShaderGpuProgramType.DX9PixelSM20:
			case ShaderGpuProgramType.DX9PixelSM30:
				sb.Append("// shader disassembly not supported on DXBC");
				break;
			case ShaderGpuProgramType.DX10Level9Vertex:
			case ShaderGpuProgramType.DX10Level9Pixel:
			case ShaderGpuProgramType.DX11VertexSM40:
			case ShaderGpuProgramType.DX11VertexSM50:
			case ShaderGpuProgramType.DX11PixelSM40:
			case ShaderGpuProgramType.DX11PixelSM50:
			case ShaderGpuProgramType.DX11GeometrySM40:
			case ShaderGpuProgramType.DX11GeometrySM50:
			case ShaderGpuProgramType.DX11HullSM50:
			case ShaderGpuProgramType.DX11DomainSM50:
				sb.Append("// shader disassembly not supported on DXBC");
				break;
			case ShaderGpuProgramType.MetalVS:
			case ShaderGpuProgramType.MetalFS:
			{
				using (BinaryReader reader = new BinaryReader(new MemoryStream(m_ProgramCode)))
				{
					if (reader.ReadUInt32() == 4027435774u)
					{
						int offset = reader.ReadInt32();
						reader.BaseStream.Position = offset;
					}
					reader.ReadStringToNull();
					byte[] buff = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
					sb.Append(Encoding.UTF8.GetString(buff));
				}
				break;
			}
			case ShaderGpuProgramType.SPIRV:
				try
				{
					sb.Append(SpirVShaderConverter.Convert(m_ProgramCode));
				}
				catch (Exception e)
				{
					sb.Append("// disassembly error " + e.Message + "\n");
				}
				break;
			case ShaderGpuProgramType.ConsoleVS:
			case ShaderGpuProgramType.ConsoleFS:
			case ShaderGpuProgramType.ConsoleHS:
			case ShaderGpuProgramType.ConsoleDS:
			case ShaderGpuProgramType.ConsoleGS:
				sb.Append(Encoding.UTF8.GetString(m_ProgramCode));
				break;
			default:
				sb.Append($"//shader disassembly not supported on {m_ProgramType}");
				break;
			}
		}
		sb.Append('"');
		return sb.ToString();
	}
}
