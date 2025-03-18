using System.Collections.Generic;

namespace AssetStudio;

public class Clip
{
	public StreamedClip m_StreamedClip;

	public DenseClip m_DenseClip;

	public ConstantClip m_ConstantClip;

	public ValueArrayConstant m_Binding;

	public Clip(ObjectReader reader)
	{
		int[] version = reader.version;
		m_StreamedClip = new StreamedClip(reader);
		m_DenseClip = new DenseClip(reader);
		if (version[0] > 4 || (version[0] == 4 && version[1] >= 3))
		{
			m_ConstantClip = new ConstantClip(reader);
		}
		if (version[0] < 2018 || (version[0] == 2018 && version[1] < 3))
		{
			m_Binding = new ValueArrayConstant(reader);
		}
	}

	public AnimationClipBindingConstant ConvertValueArrayToGenericBinding()
	{
		AnimationClipBindingConstant bindings = new AnimationClipBindingConstant();
		List<GenericBinding> genericBindings = new List<GenericBinding>();
		ValueArrayConstant values = m_Binding;
		int i = 0;
		while (i < values.m_ValueArray.Length)
		{
			uint curveID = values.m_ValueArray[i].m_ID;
			uint curveTypeID = values.m_ValueArray[i].m_TypeID;
			GenericBinding binding = new GenericBinding();
			genericBindings.Add(binding);
			switch (curveTypeID)
			{
			case 4174552735u:
				binding.path = curveID;
				binding.attribute = 1u;
				binding.typeID = ClassIDType.Transform;
				i += 3;
				break;
			case 2211994246u:
				binding.path = curveID;
				binding.attribute = 2u;
				binding.typeID = ClassIDType.Transform;
				i += 4;
				break;
			case 1512518241u:
				binding.path = curveID;
				binding.attribute = 3u;
				binding.typeID = ClassIDType.Transform;
				i += 3;
				break;
			default:
				binding.typeID = ClassIDType.Animator;
				binding.path = 0u;
				binding.attribute = curveID;
				i++;
				break;
			}
		}
		bindings.genericBindings = genericBindings.ToArray();
		return bindings;
	}
}
