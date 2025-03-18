using System;
using System.Collections.Generic;

namespace AssetStudio;

public sealed class PPtr<T> where T : Object
{
	public int m_FileID;

	public long m_PathID;

	private SerializedFile assetsFile;

	private int index = -2;

	public bool IsNull
	{
		get
		{
			if (m_PathID != 0L)
			{
				return m_FileID < 0;
			}
			return true;
		}
	}

	public PPtr(ObjectReader reader)
	{
		m_FileID = reader.ReadInt32();
		m_PathID = ((reader.m_Version < SerializedFileFormatVersion.Unknown_14) ? reader.ReadInt32() : reader.ReadInt64());
		assetsFile = reader.assetsFile;
	}

	private bool TryGetAssetsFile(out SerializedFile result)
	{
		result = null;
		if (m_FileID == 0)
		{
			result = assetsFile;
			return true;
		}
		if (m_FileID > 0 && m_FileID - 1 < assetsFile.m_Externals.Count)
		{
			AssetsManager assetsManager = assetsFile.assetsManager;
			List<SerializedFile> assetsFileList = assetsManager.assetsFileList;
			Dictionary<string, int> assetsFileIndexCache = assetsManager.assetsFileIndexCache;
			if (index == -2)
			{
				FileIdentifier m_External = assetsFile.m_Externals[m_FileID - 1];
				string name = m_External.fileName;
				if (!assetsFileIndexCache.TryGetValue(name, out index))
				{
					index = assetsFileList.FindIndex((SerializedFile x) => x.fileName.Equals(name, StringComparison.OrdinalIgnoreCase));
					assetsFileIndexCache.Add(name, index);
				}
			}
			if (index >= 0)
			{
				result = assetsFileList[index];
				return true;
			}
		}
		return false;
	}

	public bool TryGet(out T result)
	{
		if (TryGetAssetsFile(out var sourceFile) && sourceFile.ObjectsDic.TryGetValue(m_PathID, out var obj) && obj is T variable)
		{
			result = variable;
			return true;
		}
		result = null;
		return false;
	}

	public bool TryGet<T2>(out T2 result) where T2 : Object
	{
		if (TryGetAssetsFile(out var sourceFile) && sourceFile.ObjectsDic.TryGetValue(m_PathID, out var obj) && obj is T2 variable)
		{
			result = variable;
			return true;
		}
		result = null;
		return false;
	}

	public void Set(T m_Object)
	{
		string name = m_Object.assetsFile.fileName;
		if (string.Equals(assetsFile.fileName, name, StringComparison.OrdinalIgnoreCase))
		{
			m_FileID = 0;
		}
		else
		{
			m_FileID = assetsFile.m_Externals.FindIndex((FileIdentifier x) => string.Equals(x.fileName, name, StringComparison.OrdinalIgnoreCase));
			if (m_FileID == -1)
			{
				assetsFile.m_Externals.Add(new FileIdentifier
				{
					fileName = m_Object.assetsFile.fileName
				});
				m_FileID = assetsFile.m_Externals.Count;
			}
			else
			{
				m_FileID++;
			}
		}
		AssetsManager assetsManager = assetsFile.assetsManager;
		List<SerializedFile> assetsFileList = assetsManager.assetsFileList;
		Dictionary<string, int> assetsFileIndexCache = assetsManager.assetsFileIndexCache;
		if (!assetsFileIndexCache.TryGetValue(name, out index))
		{
			index = assetsFileList.FindIndex((SerializedFile x) => x.fileName.Equals(name, StringComparison.OrdinalIgnoreCase));
			assetsFileIndexCache.Add(name, index);
		}
		m_PathID = m_Object.m_PathID;
	}
}
