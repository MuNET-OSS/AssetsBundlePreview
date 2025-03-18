using System.Collections.Generic;
using System.Linq;

namespace AssetStudio;

public sealed class Avatar : NamedObject
{
	public uint m_AvatarSize;

	public AvatarConstant m_Avatar;

	public KeyValuePair<uint, string>[] m_TOS;

	public Avatar(ObjectReader reader)
		: base(reader)
	{
		m_AvatarSize = reader.ReadUInt32();
		m_Avatar = new AvatarConstant(reader);
		int numTOS = reader.ReadInt32();
		m_TOS = new KeyValuePair<uint, string>[numTOS];
		for (int i = 0; i < numTOS; i++)
		{
			m_TOS[i] = new KeyValuePair<uint, string>(reader.ReadUInt32(), reader.ReadAlignedString());
		}
	}

	public string FindBonePath(uint hash)
	{
		return m_TOS.FirstOrDefault((KeyValuePair<uint, string> pair) => pair.Key == hash).Value;
	}
}
