using System;
using System.Collections.Generic;

namespace AssetStudio;

public class ImportedFrame
{
	private List<ImportedFrame> children;

	public string Name { get; set; }

	public Vector3 LocalRotation { get; set; }

	public Vector3 LocalPosition { get; set; }

	public Vector3 LocalScale { get; set; }

	public ImportedFrame Parent { get; set; }

	public ImportedFrame this[int i] => children[i];

	public int Count => children.Count;

	public string Path
	{
		get
		{
			ImportedFrame frame = this;
			string path = frame.Name;
			while (frame.Parent != null)
			{
				frame = frame.Parent;
				path = frame.Name + "/" + path;
			}
			return path;
		}
	}

	public ImportedFrame(int childrenCount = 0)
	{
		children = new List<ImportedFrame>(childrenCount);
	}

	public void AddChild(ImportedFrame obj)
	{
		children.Add(obj);
		obj.Parent?.Remove(obj);
		obj.Parent = this;
	}

	public void Remove(ImportedFrame frame)
	{
		children.Remove(frame);
	}

	public ImportedFrame FindFrameByPath(string path)
	{
		string name = path.Substring(path.LastIndexOf('/') + 1);
		foreach (ImportedFrame frame in FindChilds(name))
		{
			if (frame.Path.EndsWith(path, StringComparison.Ordinal))
			{
				return frame;
			}
		}
		return null;
	}

	public ImportedFrame FindRelativeFrameWithPath(string path)
	{
		string[] subs = path.Split(new char[1] { '/' }, 2);
		foreach (ImportedFrame child in children)
		{
			if (child.Name == subs[0])
			{
				if (subs.Length == 1)
				{
					return child;
				}
				ImportedFrame result = child.FindRelativeFrameWithPath(subs[1]);
				if (result != null)
				{
					return result;
				}
			}
		}
		return null;
	}

	public ImportedFrame FindFrame(string name)
	{
		if (Name == name)
		{
			return this;
		}
		foreach (ImportedFrame child in children)
		{
			ImportedFrame frame = child.FindFrame(name);
			if (frame != null)
			{
				return frame;
			}
		}
		return null;
	}

	public ImportedFrame FindChild(string name, bool recursive = true)
	{
		foreach (ImportedFrame child in children)
		{
			if (recursive)
			{
				ImportedFrame frame = child.FindFrame(name);
				if (frame != null)
				{
					return frame;
				}
			}
			else if (child.Name == name)
			{
				return child;
			}
		}
		return null;
	}

	public IEnumerable<ImportedFrame> FindChilds(string name)
	{
		if (Name == name)
		{
			yield return this;
		}
		foreach (ImportedFrame child in children)
		{
			foreach (ImportedFrame item in child.FindChilds(name))
			{
				yield return item;
			}
		}
	}
}
