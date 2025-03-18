using System.IO;
using System.Linq;

namespace AssetStudio;

public class FileReader : EndianBinaryReader
{
	public string FullPath;

	public string FileName;

	public FileType FileType;

	private static readonly byte[] gzipMagic = new byte[2] { 31, 139 };

	private static readonly byte[] brotliMagic = new byte[6] { 98, 114, 111, 116, 108, 105 };

	private static readonly byte[] zipMagic = new byte[4] { 80, 75, 3, 4 };

	private static readonly byte[] zipSpannedMagic = new byte[4] { 80, 75, 7, 8 };

	public FileReader(string path)
		: this(path, File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
	{
	}

	public FileReader(string path, Stream stream)
		: base(stream)
	{
		FullPath = Path.GetFullPath(path);
		FileName = Path.GetFileName(path);
		FileType = CheckFileType();
	}

	private FileType CheckFileType()
	{
		string signature = this.ReadStringToNull(20);
		base.Position = 0L;
		switch (signature)
		{
		case "UnityWeb":
		case "UnityRaw":
		case "UnityArchive":
		case "UnityFS":
			return FileType.BundleFile;
		case "UnityWebData1.0":
			return FileType.WebFile;
		default:
		{
			byte[] magic = ReadBytes(2);
			base.Position = 0L;
			if (gzipMagic.SequenceEqual(magic))
			{
				return FileType.GZipFile;
			}
			base.Position = 32L;
			magic = ReadBytes(6);
			base.Position = 0L;
			if (brotliMagic.SequenceEqual(magic))
			{
				return FileType.BrotliFile;
			}
			if (IsSerializedFile())
			{
				return FileType.AssetsFile;
			}
			magic = ReadBytes(4);
			base.Position = 0L;
			if (zipMagic.SequenceEqual(magic) || zipSpannedMagic.SequenceEqual(magic))
			{
				return FileType.ZipFile;
			}
			return FileType.ResourceFile;
		}
		}
	}

	private bool IsSerializedFile()
	{
		long fileSize = BaseStream.Length;
		if (fileSize < 20)
		{
			return false;
		}
		ReadUInt32();
		long m_FileSize = ReadUInt32();
		uint num = ReadUInt32();
		long m_DataOffset = ReadUInt32();
		ReadByte();
		ReadBytes(3);
		if (num >= 22)
		{
			if (fileSize < 48)
			{
				base.Position = 0L;
				return false;
			}
			ReadUInt32();
			m_FileSize = ReadInt64();
			m_DataOffset = ReadInt64();
		}
		base.Position = 0L;
		if (m_FileSize != fileSize)
		{
			return false;
		}
		if (m_DataOffset > fileSize)
		{
			return false;
		}
		return true;
	}
}
