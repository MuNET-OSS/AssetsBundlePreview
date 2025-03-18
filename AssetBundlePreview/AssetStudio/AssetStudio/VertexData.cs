using System.Collections;
using System.Linq;

namespace AssetStudio;

public class VertexData
{
	public uint m_CurrentChannels;

	public uint m_VertexCount;

	public ChannelInfo[] m_Channels;

	public StreamInfo[] m_Streams;

	public byte[] m_DataSize;

	public VertexData(ObjectReader reader)
	{
		int[] version = reader.version;
		if (version[0] < 2018)
		{
			m_CurrentChannels = reader.ReadUInt32();
		}
		m_VertexCount = reader.ReadUInt32();
		if (version[0] >= 4)
		{
			int m_ChannelsSize = reader.ReadInt32();
			m_Channels = new ChannelInfo[m_ChannelsSize];
			for (int j = 0; j < m_ChannelsSize; j++)
			{
				m_Channels[j] = new ChannelInfo(reader);
			}
		}
		if (version[0] < 5)
		{
			if (version[0] < 4)
			{
				m_Streams = new StreamInfo[4];
			}
			else
			{
				m_Streams = new StreamInfo[reader.ReadInt32()];
			}
			for (int i = 0; i < m_Streams.Length; i++)
			{
				m_Streams[i] = new StreamInfo(reader);
			}
			if (version[0] < 4)
			{
				GetChannels(version);
			}
		}
		else
		{
			GetStreams(version);
		}
		m_DataSize = reader.ReadUInt8Array();
		reader.AlignStream();
	}

	private void GetStreams(int[] version)
	{
		int streamCount = m_Channels.Max((ChannelInfo x) => x.stream) + 1;
		m_Streams = new StreamInfo[streamCount];
		uint offset = 0u;
		for (int s = 0; s < streamCount; s++)
		{
			uint chnMask = 0u;
			uint stride = 0u;
			for (int chn = 0; chn < m_Channels.Length; chn++)
			{
				ChannelInfo m_Channel = m_Channels[chn];
				if (m_Channel.stream == s && m_Channel.dimension > 0)
				{
					chnMask |= (uint)(1 << chn);
					stride += m_Channel.dimension * MeshHelper.GetFormatSize(MeshHelper.ToVertexFormat(m_Channel.format, version));
				}
			}
			m_Streams[s] = new StreamInfo
			{
				channelMask = chnMask,
				offset = offset,
				stride = stride,
				dividerOp = 0,
				frequency = 0
			};
			offset += m_VertexCount * stride;
			offset = (offset + 15) & 0xFFFFFFF0u;
		}
	}

	private void GetChannels(int[] version)
	{
		m_Channels = new ChannelInfo[6];
		for (int j = 0; j < 6; j++)
		{
			m_Channels[j] = new ChannelInfo();
		}
		for (int s = 0; s < m_Streams.Length; s++)
		{
			StreamInfo m_Stream = m_Streams[s];
			BitArray channelMask = new BitArray(new int[1] { (int)m_Stream.channelMask });
			byte offset = 0;
			for (int i = 0; i < 6; i++)
			{
				if (channelMask.Get(i))
				{
					ChannelInfo m_Channel = m_Channels[i];
					m_Channel.stream = (byte)s;
					m_Channel.offset = offset;
					switch (i)
					{
					case 0:
					case 1:
						m_Channel.format = 0;
						m_Channel.dimension = 3;
						break;
					case 2:
						m_Channel.format = 2;
						m_Channel.dimension = 4;
						break;
					case 3:
					case 4:
						m_Channel.format = 0;
						m_Channel.dimension = 2;
						break;
					case 5:
						m_Channel.format = 0;
						m_Channel.dimension = 4;
						break;
					}
					offset += (byte)(m_Channel.dimension * MeshHelper.GetFormatSize(MeshHelper.ToVertexFormat(m_Channel.format, version)));
				}
			}
		}
	}
}
