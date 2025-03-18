using System.Linq;

namespace AssetStudio;

public class Shader : NamedObject
{
	public byte[] m_Script;

	public uint decompressedSize;

	public byte[] m_SubProgramBlob;

	public SerializedShader m_ParsedForm;

	public ShaderCompilerPlatform[] platforms;

	public uint[][] offsets;

	public uint[][] compressedLengths;

	public uint[][] decompressedLengths;

	public byte[] compressedBlob;

	public Shader(ObjectReader reader)
		: base(reader)
	{
		if ((version[0] == 5 && version[1] >= 5) || version[0] > 5)
		{
			m_ParsedForm = new SerializedShader(reader);
			platforms = (from x in reader.ReadUInt32Array()
				select (ShaderCompilerPlatform)x).ToArray();
			if (version[0] > 2019 || (version[0] == 2019 && version[1] >= 3))
			{
				offsets = reader.ReadUInt32ArrayArray();
				compressedLengths = reader.ReadUInt32ArrayArray();
				decompressedLengths = reader.ReadUInt32ArrayArray();
			}
			else
			{
				offsets = (from x in reader.ReadUInt32Array()
					select new uint[1] { x }).ToArray();
				compressedLengths = (from x in reader.ReadUInt32Array()
					select new uint[1] { x }).ToArray();
				decompressedLengths = (from x in reader.ReadUInt32Array()
					select new uint[1] { x }).ToArray();
			}
			compressedBlob = reader.ReadUInt8Array();
			reader.AlignStream();
			int m_DependenciesCount = reader.ReadInt32();
			for (int j = 0; j < m_DependenciesCount; j++)
			{
				new PPtr<Shader>(reader);
			}
			if (version[0] >= 2018)
			{
				int m_NonModifiableTexturesCount = reader.ReadInt32();
				for (int i = 0; i < m_NonModifiableTexturesCount; i++)
				{
					reader.ReadAlignedString();
					new PPtr<Texture>(reader);
				}
			}
			reader.ReadBoolean();
			reader.AlignStream();
		}
		else
		{
			m_Script = reader.ReadUInt8Array();
			reader.AlignStream();
			reader.ReadAlignedString();
			if (version[0] == 5 && version[1] >= 3)
			{
				decompressedSize = reader.ReadUInt32();
				m_SubProgramBlob = reader.ReadUInt8Array();
			}
		}
	}
}
