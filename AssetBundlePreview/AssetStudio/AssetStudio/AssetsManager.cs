using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace AssetStudio;

public class AssetsManager
{
	public string SpecifyUnityVersion;

	public List<SerializedFile> assetsFileList = new List<SerializedFile>();

	internal Dictionary<string, int> assetsFileIndexCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	internal Dictionary<string, BinaryReader> resourceFileReaders = new Dictionary<string, BinaryReader>(StringComparer.OrdinalIgnoreCase);

	public List<string> importFiles = new List<string>();

	public HashSet<string> importFilesHash = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private HashSet<string> noexistFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private HashSet<string> assetsFileListHash = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	public void LoadFiles(params string[] files)
	{
		ImportHelper.MergeSplitAssets(Path.GetDirectoryName(Path.GetFullPath(files[0])));
		string[] files2 = ImportHelper.ProcessingSplitFiles(files.ToList());
		Load(files2);
	}

	public void LoadFolder(string path)
	{
		ImportHelper.MergeSplitAssets(path, allDirectories: true);
		string[] files = ImportHelper.ProcessingSplitFiles(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList());
		Load(files);
	}

	private void Load(string[] files)
	{
		foreach (string text in files)
		{
			importFiles.Add(text);
			importFilesHash.Add(Path.GetFileName(text));
		}
		Progress.Reset();
		for (int j = 0; j < importFiles.Count; j++)
		{
			LoadFile(importFiles[j]);
			Progress.Report(j + 1, importFiles.Count);
		}
		importFiles.Clear();
		importFilesHash.Clear();
		noexistFiles.Clear();
		assetsFileListHash.Clear();
		ReadAssets();
		ProcessAssets();
	}

	private void LoadFile(string fullName)
	{
		FileReader reader = new FileReader(fullName);
		LoadFile(reader);
	}

	public void LoadFile(FileReader reader)
	{
		switch (reader.FileType)
		{
		case FileType.AssetsFile:
			LoadAssetsFile(reader);
			break;
		case FileType.BundleFile:
			LoadBundleFile(reader);
			break;
		case FileType.WebFile:
			LoadWebFile(reader);
			break;
		case FileType.GZipFile:
			LoadFile(ImportHelper.DecompressGZip(reader));
			break;
		case FileType.BrotliFile:
			LoadFile(ImportHelper.DecompressBrotli(reader));
			break;
		case FileType.ZipFile:
			LoadZipFile(reader);
			break;
		case FileType.ResourceFile:
			break;
		}
	}

	private void LoadAssetsFile(FileReader reader)
	{
		if (!assetsFileListHash.Contains(reader.FileName))
		{
			Logger.Info("Loading " + reader.FullPath);
			try
			{
				SerializedFile serializedFile = new SerializedFile(reader, this);
				CheckStrippedVersion(serializedFile);
				assetsFileList.Add(serializedFile);
				assetsFileListHash.Add(serializedFile.fileName);
				foreach (FileIdentifier external in serializedFile.m_Externals)
				{
					string fileName = external.fileName;
					if (importFilesHash.Contains(fileName))
					{
						continue;
					}
					string text = Path.Combine(Path.GetDirectoryName(reader.FullPath), fileName);
					if (noexistFiles.Contains(text))
					{
						continue;
					}
					if (!File.Exists(text))
					{
						string[] files = Directory.GetFiles(Path.GetDirectoryName(reader.FullPath), fileName, SearchOption.AllDirectories);
						if (files.Length != 0)
						{
							text = files[0];
						}
					}
					if (File.Exists(text))
					{
						importFiles.Add(text);
						importFilesHash.Add(fileName);
					}
					else
					{
						noexistFiles.Add(text);
					}
				}
				return;
			}
			catch (Exception e)
			{
				Logger.Error("Error while reading assets file " + reader.FullPath, e);
				reader.Dispose();
				return;
			}
		}
		Logger.Info("Skipping " + reader.FullPath);
		reader.Dispose();
	}

	private void LoadAssetsFromMemory(FileReader reader, string originalPath, string unityVersion = null)
	{
		if (!assetsFileListHash.Contains(reader.FileName))
		{
			try
			{
				SerializedFile serializedFile = new SerializedFile(reader, this);
				serializedFile.originalPath = originalPath;
				if (!string.IsNullOrEmpty(unityVersion) && serializedFile.header.m_Version < SerializedFileFormatVersion.Unknown_7)
				{
					serializedFile.SetVersion(unityVersion);
				}
				CheckStrippedVersion(serializedFile);
				assetsFileList.Add(serializedFile);
				assetsFileListHash.Add(serializedFile.fileName);
				return;
			}
			catch (Exception e)
			{
				Logger.Error("Error while reading assets file " + reader.FullPath + " from " + Path.GetFileName(originalPath), e);
				resourceFileReaders.Add(reader.FileName, reader);
				return;
			}
		}
		Logger.Info("Skipping " + originalPath + " (" + reader.FileName + ")");
	}

	private void LoadBundleFile(FileReader reader, string originalPath = null)
	{
		Logger.Info("Loading " + reader.FullPath);
		try
		{
			BundleFile bundleFile = new BundleFile(reader);
			StreamFile[] fileList = bundleFile.fileList;
			foreach (StreamFile streamFile in fileList)
			{
				FileReader fileReader = new FileReader(Path.Combine(Path.GetDirectoryName(reader.FullPath), streamFile.fileName), streamFile.stream);
				if (fileReader.FileType == FileType.AssetsFile)
				{
					LoadAssetsFromMemory(fileReader, originalPath ?? reader.FullPath, bundleFile.m_Header.unityRevision);
				}
				else
				{
					resourceFileReaders[streamFile.fileName] = fileReader;
				}
			}
		}
		catch (Exception e)
		{
			string text = "Error while reading bundle file " + reader.FullPath;
			if (originalPath != null)
			{
				text = text + " from " + Path.GetFileName(originalPath);
			}
			Logger.Error(text, e);
		}
		finally
		{
			// reader.Dispose();
		}
	}

	private void LoadWebFile(FileReader reader)
	{
		Logger.Info("Loading " + reader.FullPath);
		try
		{
			StreamFile[] fileList = new WebFile(reader).fileList;
			foreach (StreamFile streamFile in fileList)
			{
				FileReader fileReader = new FileReader(Path.Combine(Path.GetDirectoryName(reader.FullPath), streamFile.fileName), streamFile.stream);
				switch (fileReader.FileType)
				{
				case FileType.AssetsFile:
					LoadAssetsFromMemory(fileReader, reader.FullPath);
					break;
				case FileType.BundleFile:
					LoadBundleFile(fileReader, reader.FullPath);
					break;
				case FileType.WebFile:
					LoadWebFile(fileReader);
					break;
				case FileType.ResourceFile:
					resourceFileReaders[streamFile.fileName] = fileReader;
					break;
				}
			}
		}
		catch (Exception e)
		{
			Logger.Error("Error while reading web file " + reader.FullPath, e);
		}
		finally
		{
			reader.Dispose();
		}
	}

	private void LoadZipFile(FileReader reader)
	{
		Logger.Info("Loading " + reader.FileName);
		try
		{
			using ZipArchive zipArchive = new ZipArchive(reader.BaseStream, ZipArchiveMode.Read);
			List<string> list = new List<string>();
			foreach (ZipArchiveEntry entry2 in zipArchive.Entries)
			{
				if (entry2.Name.Contains(".split"))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(entry2.Name);
					string item = Path.Combine(Path.GetDirectoryName(entry2.FullName), fileNameWithoutExtension);
					if (!list.Contains(item))
					{
						list.Add(item);
						importFilesHash.Add(fileNameWithoutExtension);
					}
				}
				else
				{
					importFilesHash.Add(entry2.Name);
				}
			}
			foreach (string item2 in list)
			{
				try
				{
					Stream stream = new MemoryStream();
					int num = 0;
					while (true)
					{
						string entryName = $"{item2}.split{num++}";
						ZipArchiveEntry entry = zipArchive.GetEntry(entryName);
						if (entry == null)
						{
							break;
						}
						using Stream stream2 = entry.Open();
						stream2.CopyTo(stream);
					}
					stream.Seek(0L, SeekOrigin.Begin);
					FileReader reader2 = new FileReader(item2, stream);
					LoadFile(reader2);
				}
				catch (Exception e)
				{
					Logger.Error("Error while reading zip split file " + item2, e);
				}
			}
			foreach (ZipArchiveEntry entry3 in zipArchive.Entries)
			{
				try
				{
					string path = Path.Combine(Path.GetDirectoryName(reader.FullPath), reader.FileName, entry3.FullName);
					Stream stream3 = new MemoryStream();
					using (Stream stream4 = entry3.Open())
					{
						stream4.CopyTo(stream3);
					}
					stream3.Position = 0L;
					FileReader fileReader = new FileReader(path, stream3);
					LoadFile(fileReader);
					if (fileReader.FileType == FileType.ResourceFile)
					{
						fileReader.Position = 0L;
						if (!resourceFileReaders.ContainsKey(entry3.Name))
						{
							resourceFileReaders.Add(entry3.Name, fileReader);
						}
					}
				}
				catch (Exception e2)
				{
					Logger.Error("Error while reading zip entry " + entry3.FullName, e2);
				}
			}
		}
		catch (Exception e3)
		{
			Logger.Error("Error while reading zip file " + reader.FileName, e3);
		}
		finally
		{
			reader.Dispose();
		}
	}

	public void CheckStrippedVersion(SerializedFile assetsFile)
	{
		if (assetsFile.IsVersionStripped && string.IsNullOrEmpty(SpecifyUnityVersion))
		{
			throw new Exception("The Unity version has been stripped, please set the version in the options");
		}
		if (!string.IsNullOrEmpty(SpecifyUnityVersion))
		{
			assetsFile.SetVersion(SpecifyUnityVersion);
		}
	}

	public void Clear()
	{
		foreach (SerializedFile assetsFile in assetsFileList)
		{
			assetsFile.Objects.Clear();
			assetsFile.reader.Close();
		}
		assetsFileList.Clear();
		foreach (KeyValuePair<string, BinaryReader> resourceFileReader in resourceFileReaders)
		{
			resourceFileReader.Value.Close();
		}
		resourceFileReaders.Clear();
		assetsFileIndexCache.Clear();
	}

	public void ReadAssets()
	{
		Logger.Info("Read assets...");
		int total = assetsFileList.Sum((SerializedFile x) => x.m_Objects.Count);
		int num = 0;
		Progress.Reset();
		foreach (SerializedFile assetsFile in assetsFileList)
		{
			foreach (ObjectInfo @object in assetsFile.m_Objects)
			{
				ObjectReader objectReader = new ObjectReader(assetsFile.reader, assetsFile, @object);
				try
				{
					assetsFile.AddObject(objectReader.type switch
					{
						ClassIDType.GameObject => new GameObject(objectReader), 
						ClassIDType.Transform => new Transform(objectReader), 
						ClassIDType.Material => new Material(objectReader), 
						ClassIDType.MeshRenderer => new MeshRenderer(objectReader), 
						ClassIDType.Texture2D => new Texture2D(objectReader), 
						ClassIDType.MeshFilter => new MeshFilter(objectReader), 
						ClassIDType.Mesh => new Mesh(objectReader), 
						ClassIDType.Shader => new Shader(objectReader), 
						ClassIDType.TextAsset => new TextAsset(objectReader), 
						ClassIDType.AnimationClip => new AnimationClip(objectReader), 
						ClassIDType.AudioClip => new AudioClip(objectReader), 
						ClassIDType.Avatar => new Avatar(objectReader), 
						ClassIDType.AnimatorController => new AnimatorController(objectReader), 
						ClassIDType.Animator => new Animator(objectReader), 
						ClassIDType.Animation => new Animation(objectReader), 
						ClassIDType.MonoBehaviour => new MonoBehaviour(objectReader), 
						ClassIDType.MonoScript => new MonoScript(objectReader), 
						ClassIDType.Font => new Font(objectReader), 
						ClassIDType.PlayerSettings => new PlayerSettings(objectReader), 
						ClassIDType.SkinnedMeshRenderer => new SkinnedMeshRenderer(objectReader), 
						ClassIDType.AssetBundle => new AssetBundle(objectReader), 
						ClassIDType.ResourceManager => new ResourceManager(objectReader), 
						ClassIDType.MovieTexture => new MovieTexture(objectReader), 
						ClassIDType.Sprite => new Sprite(objectReader), 
						ClassIDType.AnimatorOverrideController => new AnimatorOverrideController(objectReader), 
						ClassIDType.RectTransform => new RectTransform(objectReader), 
						ClassIDType.VideoClip => new VideoClip(objectReader), 
						ClassIDType.SpriteAtlas => new SpriteAtlas(objectReader), 
						_ => new Object(objectReader), 
					});
				}
				catch (Exception value)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Unable to load object").AppendLine("Assets " + assetsFile.fileName).AppendLine("Path " + assetsFile.originalPath)
						.AppendLine($"Type {objectReader.type}")
						.AppendLine($"PathID {@object.m_PathID}")
						.Append(value);
					Logger.Error(stringBuilder.ToString());
				}
				Progress.Report(++num, total);
			}
		}
	}

	public void ProcessAssets()
	{
		Logger.Info("Process Assets...");
		foreach (SerializedFile assetsFile in assetsFileList)
		{
			foreach (Object @object in assetsFile.Objects)
			{
				if (@object is GameObject { m_Components: var components } gameObject)
				{
					for (int i = 0; i < components.Length; i++)
					{
						if (!components[i].TryGet(out var result))
						{
							continue;
						}
						if (!(result is Transform transform))
						{
							if (!(result is MeshRenderer meshRenderer))
							{
								if (!(result is MeshFilter meshFilter))
								{
									if (!(result is SkinnedMeshRenderer skinnedMeshRenderer))
									{
										if (!(result is Animator animator))
										{
											if (result is Animation animation)
											{
												gameObject.m_Animation = animation;
											}
										}
										else
										{
											gameObject.m_Animator = animator;
										}
									}
									else
									{
										gameObject.m_SkinnedMeshRenderer = skinnedMeshRenderer;
									}
								}
								else
								{
									gameObject.m_MeshFilter = meshFilter;
								}
							}
							else
							{
								gameObject.m_MeshRenderer = meshRenderer;
							}
						}
						else
						{
							gameObject.m_Transform = transform;
						}
					}
				}
				else
				{
					if (!(@object is SpriteAtlas { m_PackedSprites: var packedSprites } spriteAtlas))
					{
						continue;
					}
					for (int j = 0; j < packedSprites.Length; j++)
					{
						if (!packedSprites[j].TryGet(out var result2))
						{
							continue;
						}
						if (result2.m_SpriteAtlas.IsNull)
						{
							result2.m_SpriteAtlas.Set(spriteAtlas);
							continue;
						}
						result2.m_SpriteAtlas.TryGet(out var result3);
						if (result3.m_IsVariant)
						{
							result2.m_SpriteAtlas.Set(spriteAtlas);
						}
					}
				}
			}
		}
	}
}
