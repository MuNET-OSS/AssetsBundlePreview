using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD;

public class DSP : HandleBase
{
	public RESULT release()
	{
		RESULT num = FMOD_DSP_Release(getRaw());
		if (num == RESULT.OK)
		{
			rawPtr = IntPtr.Zero;
		}
		return num;
	}

	public RESULT getSystemObject(out System system)
	{
		system = null;
		IntPtr systemraw;
		RESULT result = FMOD_DSP_GetSystemObject(rawPtr, out systemraw);
		system = new System(systemraw);
		return result;
	}

	public RESULT addInput(DSP target, out DSPConnection connection, DSPCONNECTION_TYPE type)
	{
		connection = null;
		IntPtr dspconnectionraw;
		RESULT result = FMOD_DSP_AddInput(rawPtr, target.getRaw(), out dspconnectionraw, type);
		connection = new DSPConnection(dspconnectionraw);
		return result;
	}

	public RESULT disconnectFrom(DSP target, DSPConnection connection)
	{
		return FMOD_DSP_DisconnectFrom(rawPtr, target.getRaw(), connection.getRaw());
	}

	public RESULT disconnectAll(bool inputs, bool outputs)
	{
		return FMOD_DSP_DisconnectAll(rawPtr, inputs, outputs);
	}

	public RESULT getNumInputs(out int numinputs)
	{
		return FMOD_DSP_GetNumInputs(rawPtr, out numinputs);
	}

	public RESULT getNumOutputs(out int numoutputs)
	{
		return FMOD_DSP_GetNumOutputs(rawPtr, out numoutputs);
	}

	public RESULT getInput(int index, out DSP input, out DSPConnection inputconnection)
	{
		input = null;
		inputconnection = null;
		IntPtr dspinputraw;
		IntPtr dspconnectionraw;
		RESULT result = FMOD_DSP_GetInput(rawPtr, index, out dspinputraw, out dspconnectionraw);
		input = new DSP(dspinputraw);
		inputconnection = new DSPConnection(dspconnectionraw);
		return result;
	}

	public RESULT getOutput(int index, out DSP output, out DSPConnection outputconnection)
	{
		output = null;
		outputconnection = null;
		IntPtr dspoutputraw;
		IntPtr dspconnectionraw;
		RESULT result = FMOD_DSP_GetOutput(rawPtr, index, out dspoutputraw, out dspconnectionraw);
		output = new DSP(dspoutputraw);
		outputconnection = new DSPConnection(dspconnectionraw);
		return result;
	}

	public RESULT setActive(bool active)
	{
		return FMOD_DSP_SetActive(rawPtr, active);
	}

	public RESULT getActive(out bool active)
	{
		return FMOD_DSP_GetActive(rawPtr, out active);
	}

	public RESULT setBypass(bool bypass)
	{
		return FMOD_DSP_SetBypass(rawPtr, bypass);
	}

	public RESULT getBypass(out bool bypass)
	{
		return FMOD_DSP_GetBypass(rawPtr, out bypass);
	}

	public RESULT setWetDryMix(float prewet, float postwet, float dry)
	{
		return FMOD_DSP_SetWetDryMix(rawPtr, prewet, postwet, dry);
	}

	public RESULT getWetDryMix(out float prewet, out float postwet, out float dry)
	{
		return FMOD_DSP_GetWetDryMix(rawPtr, out prewet, out postwet, out dry);
	}

	public RESULT setChannelFormat(CHANNELMASK channelmask, int numchannels, SPEAKERMODE source_speakermode)
	{
		return FMOD_DSP_SetChannelFormat(rawPtr, channelmask, numchannels, source_speakermode);
	}

	public RESULT getChannelFormat(out CHANNELMASK channelmask, out int numchannels, out SPEAKERMODE source_speakermode)
	{
		return FMOD_DSP_GetChannelFormat(rawPtr, out channelmask, out numchannels, out source_speakermode);
	}

	public RESULT getOutputChannelFormat(CHANNELMASK inmask, int inchannels, SPEAKERMODE inspeakermode, out CHANNELMASK outmask, out int outchannels, out SPEAKERMODE outspeakermode)
	{
		return FMOD_DSP_GetOutputChannelFormat(rawPtr, inmask, inchannels, inspeakermode, out outmask, out outchannels, out outspeakermode);
	}

	public RESULT reset()
	{
		return FMOD_DSP_Reset(rawPtr);
	}

	public RESULT setParameterFloat(int index, float value)
	{
		return FMOD_DSP_SetParameterFloat(rawPtr, index, value);
	}

	public RESULT setParameterInt(int index, int value)
	{
		return FMOD_DSP_SetParameterInt(rawPtr, index, value);
	}

	public RESULT setParameterBool(int index, bool value)
	{
		return FMOD_DSP_SetParameterBool(rawPtr, index, value);
	}

	public RESULT setParameterData(int index, byte[] data)
	{
		return FMOD_DSP_SetParameterData(rawPtr, index, Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), (uint)data.Length);
	}

	public RESULT getParameterFloat(int index, out float value)
	{
		IntPtr valuestr = IntPtr.Zero;
		return FMOD_DSP_GetParameterFloat(rawPtr, index, out value, valuestr, 0);
	}

	public RESULT getParameterInt(int index, out int value)
	{
		IntPtr valuestr = IntPtr.Zero;
		return FMOD_DSP_GetParameterInt(rawPtr, index, out value, valuestr, 0);
	}

	public RESULT getParameterBool(int index, out bool value)
	{
		return FMOD_DSP_GetParameterBool(rawPtr, index, out value, IntPtr.Zero, 0);
	}

	public RESULT getParameterData(int index, out IntPtr data, out uint length)
	{
		return FMOD_DSP_GetParameterData(rawPtr, index, out data, out length, IntPtr.Zero, 0);
	}

	public RESULT getNumParameters(out int numparams)
	{
		return FMOD_DSP_GetNumParameters(rawPtr, out numparams);
	}

	public RESULT getParameterInfo(int index, out DSP_PARAMETER_DESC desc)
	{
		IntPtr descPtr;
		RESULT num = FMOD_DSP_GetParameterInfo(rawPtr, index, out descPtr);
		if (num == RESULT.OK)
		{
			desc = (DSP_PARAMETER_DESC)Marshal.PtrToStructure(descPtr, typeof(DSP_PARAMETER_DESC));
			return num;
		}
		desc = default(DSP_PARAMETER_DESC);
		return num;
	}

	public RESULT getDataParameterIndex(int datatype, out int index)
	{
		return FMOD_DSP_GetDataParameterIndex(rawPtr, datatype, out index);
	}

	public RESULT showConfigDialog(IntPtr hwnd, bool show)
	{
		return FMOD_DSP_ShowConfigDialog(rawPtr, hwnd, show);
	}

	public RESULT getInfo(StringBuilder name, out uint version, out int channels, out int configwidth, out int configheight)
	{
		IntPtr nameMem = Marshal.AllocHGlobal(32);
		RESULT result = FMOD_DSP_GetInfo(rawPtr, nameMem, out version, out channels, out configwidth, out configheight);
		StringMarshalHelper.NativeToBuilder(name, nameMem);
		Marshal.FreeHGlobal(nameMem);
		return result;
	}

	public RESULT getType(out DSP_TYPE type)
	{
		return FMOD_DSP_GetType(rawPtr, out type);
	}

	public RESULT getIdle(out bool idle)
	{
		return FMOD_DSP_GetIdle(rawPtr, out idle);
	}

	public RESULT setUserData(IntPtr userdata)
	{
		return FMOD_DSP_SetUserData(rawPtr, userdata);
	}

	public RESULT getUserData(out IntPtr userdata)
	{
		return FMOD_DSP_GetUserData(rawPtr, out userdata);
	}

	public RESULT setMeteringEnabled(bool inputEnabled, bool outputEnabled)
	{
		return FMOD_DSP_SetMeteringEnabled(rawPtr, inputEnabled, outputEnabled);
	}

	public RESULT getMeteringEnabled(out bool inputEnabled, out bool outputEnabled)
	{
		return FMOD_DSP_GetMeteringEnabled(rawPtr, out inputEnabled, out outputEnabled);
	}

	public RESULT getMeteringInfo(DSP_METERING_INFO inputInfo, DSP_METERING_INFO outputInfo)
	{
		return FMOD_DSP_GetMeteringInfo(rawPtr, inputInfo, outputInfo);
	}

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_Release(IntPtr dsp);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetSystemObject(IntPtr dsp, out IntPtr system);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_AddInput(IntPtr dsp, IntPtr target, out IntPtr connection, DSPCONNECTION_TYPE type);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_DisconnectFrom(IntPtr dsp, IntPtr target, IntPtr connection);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_DisconnectAll(IntPtr dsp, bool inputs, bool outputs);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetNumInputs(IntPtr dsp, out int numinputs);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetNumOutputs(IntPtr dsp, out int numoutputs);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetInput(IntPtr dsp, int index, out IntPtr input, out IntPtr inputconnection);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetOutput(IntPtr dsp, int index, out IntPtr output, out IntPtr outputconnection);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetActive(IntPtr dsp, bool active);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetActive(IntPtr dsp, out bool active);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetBypass(IntPtr dsp, bool bypass);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetBypass(IntPtr dsp, out bool bypass);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetWetDryMix(IntPtr dsp, float prewet, float postwet, float dry);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetWetDryMix(IntPtr dsp, out float prewet, out float postwet, out float dry);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetChannelFormat(IntPtr dsp, CHANNELMASK channelmask, int numchannels, SPEAKERMODE source_speakermode);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetChannelFormat(IntPtr dsp, out CHANNELMASK channelmask, out int numchannels, out SPEAKERMODE source_speakermode);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetOutputChannelFormat(IntPtr dsp, CHANNELMASK inmask, int inchannels, SPEAKERMODE inspeakermode, out CHANNELMASK outmask, out int outchannels, out SPEAKERMODE outspeakermode);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_Reset(IntPtr dsp);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetParameterFloat(IntPtr dsp, int index, float value);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetParameterInt(IntPtr dsp, int index, int value);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetParameterBool(IntPtr dsp, int index, bool value);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetParameterData(IntPtr dsp, int index, IntPtr data, uint length);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetParameterFloat(IntPtr dsp, int index, out float value, IntPtr valuestr, int valuestrlen);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetParameterInt(IntPtr dsp, int index, out int value, IntPtr valuestr, int valuestrlen);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetParameterBool(IntPtr dsp, int index, out bool value, IntPtr valuestr, int valuestrlen);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetParameterData(IntPtr dsp, int index, out IntPtr data, out uint length, IntPtr valuestr, int valuestrlen);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetNumParameters(IntPtr dsp, out int numparams);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetParameterInfo(IntPtr dsp, int index, out IntPtr desc);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetDataParameterIndex(IntPtr dsp, int datatype, out int index);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_ShowConfigDialog(IntPtr dsp, IntPtr hwnd, bool show);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetInfo(IntPtr dsp, IntPtr name, out uint version, out int channels, out int configwidth, out int configheight);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetType(IntPtr dsp, out DSP_TYPE type);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetIdle(IntPtr dsp, out bool idle);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_SetUserData(IntPtr dsp, IntPtr userdata);

	[DllImport("fmod")]
	private static extern RESULT FMOD_DSP_GetUserData(IntPtr dsp, out IntPtr userdata);

	[DllImport("fmod")]
	public static extern RESULT FMOD_DSP_SetMeteringEnabled(IntPtr dsp, bool inputEnabled, bool outputEnabled);

	[DllImport("fmod")]
	public static extern RESULT FMOD_DSP_GetMeteringEnabled(IntPtr dsp, out bool inputEnabled, out bool outputEnabled);

	[DllImport("fmod")]
	public static extern RESULT FMOD_DSP_GetMeteringInfo(IntPtr dsp, [Out] DSP_METERING_INFO inputInfo, [Out] DSP_METERING_INFO outputInfo);

	public DSP(IntPtr raw)
		: base(raw)
	{
	}
}
