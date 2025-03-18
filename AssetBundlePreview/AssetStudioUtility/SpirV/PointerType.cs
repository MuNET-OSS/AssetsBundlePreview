using System.Text;

namespace SpirV;

public class PointerType : Type
{
	public StorageClass StorageClass { get; }

	public Type Type { get; private set; }

	public PointerType(StorageClass storageClass, Type type)
	{
		StorageClass = storageClass;
		Type = type;
	}

	public PointerType(StorageClass storageClass)
	{
		StorageClass = storageClass;
	}

	public void ResolveForwardReference(Type t)
	{
		Type = t;
	}

	public override string ToString()
	{
		if (Type == null)
		{
			return $"{StorageClass} *";
		}
		return $"{StorageClass} {Type}*";
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		sb.Append(StorageClass.ToString()).Append(' ');
		if (Type != null)
		{
			Type.ToString(sb);
		}
		sb.Append('*');
		return sb;
	}
}
