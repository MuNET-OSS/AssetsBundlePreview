using System;
using System.Collections.Generic;

namespace AssetStudio;

public sealed class SpriteAtlas : NamedObject
{
	public PPtr<Sprite>[] m_PackedSprites;

	public Dictionary<KeyValuePair<Guid, long>, SpriteAtlasData> m_RenderDataMap;

	public bool m_IsVariant;

	public SpriteAtlas(ObjectReader reader)
		: base(reader)
	{
		int m_PackedSpritesSize = reader.ReadInt32();
		m_PackedSprites = new PPtr<Sprite>[m_PackedSpritesSize];
		for (int j = 0; j < m_PackedSpritesSize; j++)
		{
			m_PackedSprites[j] = new PPtr<Sprite>(reader);
		}
		reader.ReadStringArray();
		int m_RenderDataMapSize = reader.ReadInt32();
		m_RenderDataMap = new Dictionary<KeyValuePair<Guid, long>, SpriteAtlasData>(m_RenderDataMapSize);
		for (int i = 0; i < m_RenderDataMapSize; i++)
		{
			Guid first = new Guid(reader.ReadBytes(16));
			long second = reader.ReadInt64();
			SpriteAtlasData value = new SpriteAtlasData(reader);
			m_RenderDataMap.Add(new KeyValuePair<Guid, long>(first, second), value);
		}
		reader.ReadAlignedString();
		m_IsVariant = reader.ReadBoolean();
		reader.AlignStream();
	}
}
