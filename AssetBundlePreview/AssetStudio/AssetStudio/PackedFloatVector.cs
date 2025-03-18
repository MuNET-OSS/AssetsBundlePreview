using System;
using System.Collections.Generic;

namespace AssetStudio;

public class PackedFloatVector
{
	public uint m_NumItems;

	public float m_Range;

	public float m_Start;

	public byte[] m_Data;

	public byte m_BitSize;

	public PackedFloatVector(ObjectReader reader)
	{
		m_NumItems = reader.ReadUInt32();
		m_Range = reader.ReadSingle();
		m_Start = reader.ReadSingle();
		int numData = reader.ReadInt32();
		m_Data = reader.ReadBytes(numData);
		reader.AlignStream();
		m_BitSize = reader.ReadByte();
		reader.AlignStream();
	}

	public float[] UnpackFloats(int itemCountInChunk, int chunkStride, int start = 0, int numChunks = -1)
	{
		int bitPos = m_BitSize * start;
		int indexPos = bitPos / 8;
		bitPos %= 8;
		float scale = 1f / m_Range;
		if (numChunks == -1)
		{
			numChunks = (int)m_NumItems / itemCountInChunk;
		}
		int end = chunkStride * numChunks / 4;
		List<float> data = new List<float>();
		for (int index = 0; index != end; index += chunkStride / 4)
		{
			for (int i = 0; i < itemCountInChunk; i++)
			{
				uint x = 0u;
				int bits = 0;
				while (bits < m_BitSize)
				{
					x |= (uint)(m_Data[indexPos] >> bitPos << bits);
					int num = Math.Min(m_BitSize - bits, 8 - bitPos);
					bitPos += num;
					bits += num;
					if (bitPos == 8)
					{
						indexPos++;
						bitPos = 0;
					}
				}
				x &= (uint)((1 << (int)m_BitSize) - 1);
				data.Add((float)x / (scale * (float)((1 << (int)m_BitSize) - 1)) + m_Start);
			}
		}
		return data.ToArray();
	}
}
