using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FMOD;

public class ChannelGroup : ChannelControl
{
	public RESULT release()
	{
		RESULT num = FMOD_ChannelGroup_Release(getRaw());
		if (num == RESULT.OK)
		{
			rawPtr = IntPtr.Zero;
		}
		return num;
	}

	public RESULT addGroup(ChannelGroup group, bool propagatedspclock, out DSPConnection connection)
	{
		connection = null;
		IntPtr connectionRaw;
		RESULT result = FMOD_ChannelGroup_AddGroup(getRaw(), group.getRaw(), propagatedspclock, out connectionRaw);
		connection = new DSPConnection(connectionRaw);
		return result;
	}

	public RESULT getNumGroups(out int numgroups)
	{
		return FMOD_ChannelGroup_GetNumGroups(getRaw(), out numgroups);
	}

	public RESULT getGroup(int index, out ChannelGroup group)
	{
		group = null;
		IntPtr groupraw;
		RESULT result = FMOD_ChannelGroup_GetGroup(getRaw(), index, out groupraw);
		group = new ChannelGroup(groupraw);
		return result;
	}

	public RESULT getParentGroup(out ChannelGroup group)
	{
		group = null;
		IntPtr groupraw;
		RESULT result = FMOD_ChannelGroup_GetParentGroup(getRaw(), out groupraw);
		group = new ChannelGroup(groupraw);
		return result;
	}

	public RESULT getName(StringBuilder name, int namelen)
	{
		IntPtr stringMem = Marshal.AllocHGlobal(name.Capacity);
		RESULT result = FMOD_ChannelGroup_GetName(getRaw(), stringMem, namelen);
		StringMarshalHelper.NativeToBuilder(name, stringMem);
		Marshal.FreeHGlobal(stringMem);
		return result;
	}

	public RESULT getNumChannels(out int numchannels)
	{
		return FMOD_ChannelGroup_GetNumChannels(getRaw(), out numchannels);
	}

	public RESULT getChannel(int index, out Channel channel)
	{
		channel = null;
		IntPtr channelraw;
		RESULT result = FMOD_ChannelGroup_GetChannel(getRaw(), index, out channelraw);
		channel = new Channel(channelraw);
		return result;
	}

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_Release(IntPtr channelgroup);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group, bool propagatedspclock, out IntPtr connection);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetNumGroups(IntPtr channelgroup, out int numgroups);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetGroup(IntPtr channelgroup, int index, out IntPtr group);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetParentGroup(IntPtr channelgroup, out IntPtr group);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetName(IntPtr channelgroup, IntPtr name, int namelen);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetNumChannels(IntPtr channelgroup, out int numchannels);

	[DllImport("fmod")]
	private static extern RESULT FMOD_ChannelGroup_GetChannel(IntPtr channelgroup, int index, out IntPtr channel);

	public ChannelGroup(IntPtr raw)
		: base(raw)
	{
	}
}
