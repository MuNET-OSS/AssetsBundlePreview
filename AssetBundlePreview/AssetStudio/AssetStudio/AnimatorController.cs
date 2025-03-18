using System.Collections.Generic;

namespace AssetStudio;

public sealed class AnimatorController : RuntimeAnimatorController
{
	public PPtr<AnimationClip>[] m_AnimationClips;

	public AnimatorController(ObjectReader reader)
		: base(reader)
	{
		reader.ReadUInt32();
		new ControllerConstant(reader);
		int tosSize = reader.ReadInt32();
		KeyValuePair<uint, string>[] m_TOS = new KeyValuePair<uint, string>[tosSize];
		for (int j = 0; j < tosSize; j++)
		{
			m_TOS[j] = new KeyValuePair<uint, string>(reader.ReadUInt32(), reader.ReadAlignedString());
		}
		int numClips = reader.ReadInt32();
		m_AnimationClips = new PPtr<AnimationClip>[numClips];
		for (int i = 0; i < numClips; i++)
		{
			m_AnimationClips[i] = new PPtr<AnimationClip>(reader);
		}
	}
}
