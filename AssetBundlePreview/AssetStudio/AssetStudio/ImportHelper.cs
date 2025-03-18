using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Org.Brotli.Dec;

namespace AssetStudio;

public static class ImportHelper
{
	public static void MergeSplitAssets(string path, bool allDirectories = false)
	{
		string[] files = Directory.GetFiles(path, "*.split0", allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
		foreach (string path2 in files)
		{
			string destFile = Path.GetFileNameWithoutExtension(path2);
			string destPath = Path.GetDirectoryName(path2);
			string destFull = Path.Combine(destPath, destFile);
			if (File.Exists(destFull))
			{
				continue;
			}
			string[] splitParts = Directory.GetFiles(destPath, destFile + ".split*");
			using FileStream destStream = File.Create(destFull);
			for (int i = 0; i < splitParts.Length; i++)
			{
				using FileStream sourceStream = File.OpenRead(destFull + ".split" + i);
				sourceStream.CopyTo(destStream);
			}
		}
	}

	public static string[] ProcessingSplitFiles(List<string> selectFile)
	{
		List<string> list = (from x in selectFile
			where x.Contains(".split")
			select Path.Combine(Path.GetDirectoryName(x), Path.GetFileNameWithoutExtension(x))).Distinct().ToList();
		selectFile.RemoveAll((string x) => x.Contains(".split"));
		foreach (string file in list)
		{
			if (File.Exists(file))
			{
				selectFile.Add(file);
			}
		}
		return selectFile.Distinct().ToArray();
	}

	public static FileReader DecompressGZip(FileReader reader)
	{
		using (reader)
		{
			MemoryStream stream = new MemoryStream();
			using (GZipStream gs = new GZipStream(reader.BaseStream, CompressionMode.Decompress))
			{
				gs.CopyTo(stream);
			}
			stream.Position = 0L;
			return new FileReader(reader.FullPath, stream);
		}
	}

	public static FileReader DecompressBrotli(FileReader reader)
	{
		using (reader)
		{
			MemoryStream stream = new MemoryStream();
			using (BrotliInputStream brotliStream = new BrotliInputStream(reader.BaseStream))
			{
				brotliStream.CopyTo(stream);
			}
			stream.Position = 0L;
			return new FileReader(reader.FullPath, stream);
		}
	}
}
