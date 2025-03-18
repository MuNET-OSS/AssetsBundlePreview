using System;
using System.Runtime.InteropServices;

namespace FMOD;

public class DSPConnection : HandleBase
{
	public RESULT getInput(out DSP input)
	{
		input = null;
		IntPtr dspraw;
		RESULT result = FMOD_DSPConnection_GetInput(rawPtr, out dspraw);
		input = new DSP(dspraw);
		return result;
	}

	public RESULT getOutput(out DSP output)
	{
		output = null;
		IntPtr dspraw;
		RESULT result = FMOD_DSPConnection_GetOutput(rawPtr, out dspraw);
		output = new DSP(dspraw);
		return result;
	}

	public RESULT setMix(float volume)
	{
		return FMOD_DSPConnection_SetMix(rawPtr, volume);
	}

	public RESULT getMix(out float volume)
	{
		return FMOD_DSPConnection_GetMix(rawPtr, out volume);
	}

	public RESULT setMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannel_hop)
	{
		return FMOD_DSPConnection_SetMixMatrix(rawPtr, matrix, outchannels, inchannels, inchannel_hop);
	}

	public RESULT getMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannel_hop)
	{
		return FMOD_DSPConnection_GetMixMatrix(rawPtr, matrix, out outchannels, out inchannels, inchannel_hop);
	}

	public RESULT getType(out DSPCONNECTION_TYPE type)
	{
		return FMOD_DSPConnection_GetType(rawPtr, out type);
	}

	public RESULT setUserData(IntPtr userdata)
	{
		return FMOD_DSPConnection_SetUserData(rawPtr, userdata);
	}

	public RESULT getUserData(out IntPtr userdata)
	{
		return FMOD_DSPConnection_GetUserData(rawPtr, out userdata);
	}

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetInput(IntPtr dspconnection, out IntPtr input);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetOutput(IntPtr dspconnection, out IntPtr output);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_SetMix(IntPtr dspconnection, float volume);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetMix(IntPtr dspconnection, out float volume);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_SetMixMatrix(IntPtr dspconnection, float[] matrix, int outchannels, int inchannels, int inchannel_hop);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetMixMatrix(IntPtr dspconnection, float[] matrix, out int outchannels, out int inchannels, int inchannel_hop);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetType(IntPtr dspconnection, out DSPCONNECTION_TYPE type);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_SetUserData(IntPtr dspconnection, IntPtr userdata);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSPConnection_GetUserData(IntPtr dspconnection, out IntPtr userdata);

	public DSPConnection(IntPtr raw)
		: base(raw)
	{
	}
}
