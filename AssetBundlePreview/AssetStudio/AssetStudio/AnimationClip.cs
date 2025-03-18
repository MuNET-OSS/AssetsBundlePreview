namespace AssetStudio;

public sealed class AnimationClip : NamedObject
{
	public AnimationType m_AnimationType;

	public bool m_Legacy;

	public bool m_Compressed;

	public bool m_UseHighQualityCurve;

	public QuaternionCurve[] m_RotationCurves;

	public CompressedAnimationCurve[] m_CompressedRotationCurves;

	public Vector3Curve[] m_EulerCurves;

	public Vector3Curve[] m_PositionCurves;

	public Vector3Curve[] m_ScaleCurves;

	public FloatCurve[] m_FloatCurves;

	public PPtrCurve[] m_PPtrCurves;

	public float m_SampleRate;

	public int m_WrapMode;

	public AABB m_Bounds;

	public uint m_MuscleClipSize;

	public ClipMuscleConstant m_MuscleClip;

	public AnimationClipBindingConstant m_ClipBindingConstant;

	public AnimationEvent[] m_Events;

	public AnimationClip(ObjectReader reader)
		: base(reader)
	{
		if (version[0] >= 5)
		{
			m_Legacy = reader.ReadBoolean();
		}
		else if (version[0] >= 4)
		{
			m_AnimationType = (AnimationType)reader.ReadInt32();
			if (m_AnimationType == AnimationType.Legacy)
			{
				m_Legacy = true;
			}
		}
		else
		{
			m_Legacy = true;
		}
		m_Compressed = reader.ReadBoolean();
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_UseHighQualityCurve = reader.ReadBoolean();
		}
		reader.AlignStream();
		int numRCurves = reader.ReadInt32();
		m_RotationCurves = new QuaternionCurve[numRCurves];
		for (int i3 = 0; i3 < numRCurves; i3++)
		{
			m_RotationCurves[i3] = new QuaternionCurve(reader);
		}
		int numCRCurves = reader.ReadInt32();
		m_CompressedRotationCurves = new CompressedAnimationCurve[numCRCurves];
		for (int i2 = 0; i2 < numCRCurves; i2++)
		{
			m_CompressedRotationCurves[i2] = new CompressedAnimationCurve(reader);
		}
		if (version[0] > 5 || (version[0] == 5 && version[1] >= 3))
		{
			int numEulerCurves = reader.ReadInt32();
			m_EulerCurves = new Vector3Curve[numEulerCurves];
			for (int n = 0; n < numEulerCurves; n++)
			{
				m_EulerCurves[n] = new Vector3Curve(reader);
			}
		}
		int numPCurves = reader.ReadInt32();
		m_PositionCurves = new Vector3Curve[numPCurves];
		for (int m = 0; m < numPCurves; m++)
		{
			m_PositionCurves[m] = new Vector3Curve(reader);
		}
		int numSCurves = reader.ReadInt32();
		m_ScaleCurves = new Vector3Curve[numSCurves];
		for (int l = 0; l < numSCurves; l++)
		{
			m_ScaleCurves[l] = new Vector3Curve(reader);
		}
		int numFCurves = reader.ReadInt32();
		m_FloatCurves = new FloatCurve[numFCurves];
		for (int k = 0; k < numFCurves; k++)
		{
			m_FloatCurves[k] = new FloatCurve(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			int numPtrCurves = reader.ReadInt32();
			m_PPtrCurves = new PPtrCurve[numPtrCurves];
			for (int j = 0; j < numPtrCurves; j++)
			{
				m_PPtrCurves[j] = new PPtrCurve(reader);
			}
		}
		m_SampleRate = reader.ReadSingle();
		m_WrapMode = reader.ReadInt32();
		if (version[0] > 3 || (version[0] == 3 && version[1] >= 4))
		{
			m_Bounds = new AABB(reader);
		}
		if (version[0] >= 4)
		{
			m_MuscleClipSize = reader.ReadUInt32();
			m_MuscleClip = new ClipMuscleConstant(reader);
		}
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_ClipBindingConstant = new AnimationClipBindingConstant(reader);
		}
		if (version[0] > 2018 || (version[0] == 2018 && version[1] >= 3))
		{
			reader.ReadBoolean();
			reader.ReadBoolean();
			reader.AlignStream();
		}
		int numEvents = reader.ReadInt32();
		m_Events = new AnimationEvent[numEvents];
		for (int i = 0; i < numEvents; i++)
		{
			m_Events[i] = new AnimationEvent(reader);
		}
		if (version[0] >= 2017)
		{
			reader.AlignStream();
		}
	}
}
