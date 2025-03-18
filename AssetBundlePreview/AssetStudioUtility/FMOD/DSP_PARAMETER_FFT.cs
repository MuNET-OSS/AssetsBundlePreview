using System;
using System.Runtime.InteropServices;

namespace FMOD;

public struct DSP_PARAMETER_FFT
{
	public int length;

	public int numchannels;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	private IntPtr[] spectrum_internal;

	public float[][] spectrum
	{
		get
		{
			float[][] buffer = new float[numchannels][];
			for (int i = 0; i < numchannels; i++)
			{
				buffer[i] = new float[length];
				Marshal.Copy(spectrum_internal[i], buffer[i], 0, length);
			}
			return buffer;
		}
	}
}
