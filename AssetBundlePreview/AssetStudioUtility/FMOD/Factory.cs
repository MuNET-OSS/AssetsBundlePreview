using System;
using System.Runtime.InteropServices;
using AssetStudio.PInvoke;

namespace FMOD;

public class Factory
{
	static Factory()
	{
		DllLoader.PreloadDll("fmod");
	}

	public static RESULT System_Create(out System system)
	{
		system = null;
		RESULT result = RESULT.OK;
		IntPtr rawPtr = default(IntPtr);
		result = FMOD_System_Create(out rawPtr);
		if (result != 0)
		{
			return result;
		}
		system = new System(rawPtr);
		return result;
	}

	[DllImport("fmod")]
	private static extern RESULT FMOD_System_Create(out IntPtr system);
}
