namespace AssetStudio;

public class StateMachineConstant
{
	public StateConstant[] m_StateConstantArray;

	public TransitionConstant[] m_AnyStateTransitionConstantArray;

	public SelectorStateConstant[] m_SelectorStateConstantArray;

	public uint m_DefaultState;

	public uint m_MotionSetCount;

	public StateMachineConstant(ObjectReader reader)
	{
		int[] version = reader.version;
		int numStates = reader.ReadInt32();
		m_StateConstantArray = new StateConstant[numStates];
		for (int k = 0; k < numStates; k++)
		{
			m_StateConstantArray[k] = new StateConstant(reader);
		}
		int numAnyStates = reader.ReadInt32();
		m_AnyStateTransitionConstantArray = new TransitionConstant[numAnyStates];
		for (int j = 0; j < numAnyStates; j++)
		{
			m_AnyStateTransitionConstantArray[j] = new TransitionConstant(reader);
		}
		if (version[0] >= 5)
		{
			int numSelectors = reader.ReadInt32();
			m_SelectorStateConstantArray = new SelectorStateConstant[numSelectors];
			for (int i = 0; i < numSelectors; i++)
			{
				m_SelectorStateConstantArray[i] = new SelectorStateConstant(reader);
			}
		}
		m_DefaultState = reader.ReadUInt32();
		m_MotionSetCount = reader.ReadUInt32();
	}
}
