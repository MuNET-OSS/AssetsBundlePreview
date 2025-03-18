using System.IO;
using System.Text.RegularExpressions;

namespace AssetStudio;

public class ShaderProgram
{
	public ShaderSubProgramEntry[] entries;

	public ShaderSubProgram[] m_SubPrograms;

	public ShaderProgram(BinaryReader reader, int[] version)
	{
		int subProgramsCapacity = reader.ReadInt32();
		entries = new ShaderSubProgramEntry[subProgramsCapacity];
		for (int i = 0; i < subProgramsCapacity; i++)
		{
			entries[i] = new ShaderSubProgramEntry(reader, version);
		}
		m_SubPrograms = new ShaderSubProgram[subProgramsCapacity];
	}

	public void Read(BinaryReader reader, int segment)
	{
		for (int i = 0; i < entries.Length; i++)
		{
			ShaderSubProgramEntry entry = entries[i];
			if (entry.Segment == segment)
			{
				reader.BaseStream.Position = entry.Offset;
				m_SubPrograms[i] = new ShaderSubProgram(reader);
			}
		}
	}

	public string Export(string shader)
	{
		MatchEvaluator evaluator = delegate(Match match)
		{
			int num = int.Parse(match.Groups[1].Value);
			return m_SubPrograms[num].Export();
		};
		shader = Regex.Replace(shader, "GpuProgramIndex (.+)", evaluator);
		return shader;
	}
}
