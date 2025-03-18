using System.Collections.Generic;
using System.Text;

namespace SpirV;

public class StructType : Type
{
	private List<string> memberNames_;

	public IReadOnlyList<Type> MemberTypes { get; }

	public IReadOnlyList<string> MemberNames => memberNames_;

	public StructType(IReadOnlyList<Type> memberTypes)
	{
		MemberTypes = memberTypes;
		memberNames_ = new List<string>();
		for (int i = 0; i < memberTypes.Count; i++)
		{
			memberNames_.Add(string.Empty);
		}
	}

	public void SetMemberName(uint member, string name)
	{
		memberNames_[(int)member] = name;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		ToString(sb);
		return sb.ToString();
	}

	public override StringBuilder ToString(StringBuilder sb)
	{
		sb.Append("struct {");
		for (int i = 0; i < MemberTypes.Count; i++)
		{
			MemberTypes[i].ToString(sb);
			if (!string.IsNullOrEmpty(memberNames_[i]))
			{
				sb.Append(' ');
				sb.Append(MemberNames[i]);
			}
			sb.Append(';');
			if (i < MemberTypes.Count - 1)
			{
				sb.Append(' ');
			}
		}
		sb.Append('}');
		return sb;
	}
}
